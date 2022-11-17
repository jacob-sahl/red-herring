interface Player {
    id: number;
    name: string;
    score: number;
    session: string;
    isDetective: boolean;
    gameId: string;
}

const generateSession = () => {
    const chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz';
    let result = '';
    for (let i = 0; i < 32; i++) {
        result += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    return result;
};

export default Player;
export { generateSession };
