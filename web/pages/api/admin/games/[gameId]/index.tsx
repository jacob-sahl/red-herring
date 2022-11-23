import type { NextApiRequest, NextApiResponse } from 'next'
import NodeAppInstance from '../../../../../firebase/nodeApp';

export default function handler(
    req: NextApiRequest,
    res: NextApiResponse
) {
    if (req.method === 'GET') {
        // Query a new game instance
        const { gameId } = req.query;
        if (gameId) {
            const gameInstance = NodeAppInstance.getGameInstanceById(gameId as string);
            if (gameInstance) {
                res.json(gameInstance);
            } else {
                res.status(404).json({ message: 'Game not found' });
            }
        } else {
            res.status(400).json({ error: 'Missing gameId' });
        }
    } else if (req.method === 'DELETE') {
        // Delete a game instance
        const { gameId } = req.query;
        if (gameId) {
            const gameInstance = NodeAppInstance.getGameInstanceById(gameId as string);
            if (gameInstance) {
                NodeAppInstance.deleteGameInstance(gameInstance);
                res.json({ message: 'Game deleted' });
            } else {
                res.status(404).json({ message: 'Game not found' });
            }
        } else {
            res.status(400).json({ error: 'Missing gameId' });
        }
    }
}

