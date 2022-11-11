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

export default GameInstance;