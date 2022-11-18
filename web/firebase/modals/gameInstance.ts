import Player from './player';
import Round from './round';

interface GameInstance {
    createdTime: string;
    id: string;
    currentRound: number;
    joinCode: string;
    players: Player[];
    rounds: Round[];
}

const isJoinable = (gameInstance: GameInstance) => {
    return gameInstance.players === undefined ? true : gameInstance.players.length < 4;
};

export default GameInstance;
export { isJoinable };
