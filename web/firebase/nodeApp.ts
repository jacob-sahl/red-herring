import { Database } from '@firebase/database-types';
import * as admin from 'firebase-admin'
import GameInstance from './modals/gameInstance';
import Player from './modals/player';
import Round, { CurrentGameState } from './modals/round';

class NodeApp {

    private static _instance: NodeApp;

    public static get Instance() {
        return this._instance || (this._instance = new this());
    }

    private db: Database | undefined;

    private allGameInstances: GameInstance[] = [];

    private constructor() {
        if (!admin.apps.length) {
            admin.initializeApp({
                credential: admin.credential.cert({
                    projectId: process.env.NEXT_PUBLIC_FIREBASE_PROJECT_ID,
                    clientEmail: process.env.FIREBASE_CLIENT_EMAIL,
                    privateKey: process.env.FIREBASE_PRIVATE_KEY!.replace(/\\n/g, '\n'),
                }),
                databaseURL: process.env.NEXT_PUBLIC_FIREBASE_DATABASE_URL,
            });
        }
        this.db = admin.database();
        this.SubscribeAllGameInstances();
    }

    private async SubscribeAllGameInstances() {
        const allGameInstancesObj = (await this.db?.ref('games').once('value'))?.val() as { [key: string]: GameInstance };
        this.allGameInstances = Object.values(allGameInstancesObj);

        this.db?.ref('games').on('value', (snapshot) => {
            this.allGameInstances = Object.values(snapshot.val() as { [key: string]: GameInstance });
        });
    }

    public getGameInstanceById(id: string) {
        return this.allGameInstances.find((game) => game.id === id);
    }

    public async createGameInstance() {
        const gameInstance = {
            createdTime: new Date().toISOString(),
            id: this.db?.ref('games').push().key,
            currentRound: -1,
            joinCode: this.generateJoinCode(),
            players: [],
            rounds: [],
        };
        await this.db?.ref('games').child(gameInstance.id!).set(gameInstance);
        return gameInstance as GameInstance;
    }

    private generateJoinCode(): string {
        const chars = '0123456789';
        let result = '';
        for (let i = 0; i < 4; i++) {
            result += chars.charAt(Math.floor(Math.random() * chars.length));
        }
        if (this.allGameInstances.find((game) => game.joinCode === result)) {
            return this.generateJoinCode();
        }
        return result;
    }

    public async updateGameInstance(gameInstance: GameInstance) {
        await this.db?.ref('games').child(gameInstance.id).set(gameInstance);
    }

    public async deleteGameInstance(gameInstance: GameInstance) {
        await this.db?.ref('games').child(gameInstance.id).remove();
    }

    public async addPlayerToGameInstance(gameInstance: GameInstance, player: Player) {
        gameInstance.players === undefined ? gameInstance.players = [player] : gameInstance.players.push(player);
        await this.updateGameInstance(gameInstance);
    }

    public async getGameInstanceByJoinCode(joinCode: string) {
        return this.allGameInstances.find((game) => game.joinCode === joinCode);
    }

    public validatePlayer(gameId: string, playerId: number, session: string) {
        const gameInstance = this.getGameInstanceById(gameId);
        if (!gameInstance) {
            return false;
        }
        const player = gameInstance.players.find((player) => player.id === playerId);
        if (!player) {
            return false;
        }
        return player.session === session;
    }

    public getAllScores(gameId: string): { playerId: number; score: number; }[][] {
        const gameInstance = this.getGameInstanceById(gameId);
        if (!gameInstance) {
            return [];
        }
        if (!gameInstance.rounds) {
            return [];
        }
        return gameInstance.rounds.map((round) => round.scores).filter((scores) => scores !== undefined);
    }

    public async getPlayerRoundInfo(gameId: string, playerId: number) {
        const gameInstance = this.getGameInstanceById(gameId);
        if (!gameInstance) {
            return null;
        }
        const player = gameInstance.players.find((player) => player.id === playerId);
        if (!player) {
            return null;
        }

        const round: Round | null = gameInstance.rounds ? gameInstance.rounds[gameInstance.currentRound] : null;

        const informantCard = round?.informants.find((informant) => informant.playerId === playerId);
        const playerRoundInfo: CurrentGameState = {
            isDetective: player.isDetective ? player.isDetective : false,
            gameId: gameInstance.id,
            playerId: player.id,
            currentRound: gameInstance.currentRound,
            players: gameInstance.players.map((player) => ({ id: player.id, name: player.name })),
            scores: this.getAllScores(gameId),
            currentInformantCard: informantCard ? informantCard : null,
        }
        return playerRoundInfo;
    }

    public async getPlayerName(gameId: string, playerId: number) {
        const gameInstance = this.getGameInstanceById(gameId);
        if (!gameInstance) {
            return null;
        }
        const player = gameInstance.players.find((player) => player.id === playerId);
        if (!player) {
            return null;
        }
        return player.name;
    }
}

const NodeAppInstance = NodeApp.Instance;

export default NodeAppInstance;
