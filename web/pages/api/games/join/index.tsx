import type { NextApiRequest, NextApiResponse } from 'next'
import { isJoinable } from '../../../../firebase/modals/gameInstance';
import Player, { generateSession } from '../../../../firebase/modals/player';
import NodeAppInstance from '../../../../firebase/nodeApp';

export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse
) {
    if (req.method === 'POST') {
        // Join a game
        const { joinCode, playerName } = req.body;
        if (joinCode && playerName) {
            const gameInstance = await NodeAppInstance.getGameInstanceByJoinCode(joinCode as string);
            if (gameInstance && isJoinable(gameInstance)) {
                const player: Player = {
                    name: playerName,
                    score: 0,
                    id: gameInstance.players ? gameInstance.players.length : 0,
                    session: generateSession(),
                    isDetective: false,
                    gameId: gameInstance.id,
                };
                NodeAppInstance.addPlayerToGameInstance(gameInstance, player);
                res.json(player);
            } else {
                res.status(404).json({ message: 'Cannot join to the game' });
            }
        } else {
            res.status(400).json({ error: 'Missing gameId or playerName' });
        }
    } else {
        res.status(400).json({ error: 'Bad request' });
    }

}