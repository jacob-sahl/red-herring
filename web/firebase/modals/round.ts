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

export default Round;
export type { InformantCard };