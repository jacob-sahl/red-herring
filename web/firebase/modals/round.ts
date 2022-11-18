interface InformantCard {
    playerId: number;
    clue: string;
    secretGoal: string;
}

interface Round {
    detective: number;
    roundNumber: number;
    informants: InformantCard[];
    scores: {
        playerId: number;
        score: number;
    }[];
}

interface CurrentGameState {
    gameId: string;
    currentRound: number;
    playerId: number;
    isDetective: boolean;
    players: {
        id: number;
        name: string;
    }[];
    scores: {playerId: number, score: number}[][];
    currentInformantCard: InformantCard | null;
}

export default Round;
export type { InformantCard, CurrentGameState };