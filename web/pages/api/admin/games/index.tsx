import type { NextApiRequest, NextApiResponse } from 'next'
import NodeAppInstance from '../../../../firebase/nodeApp';

export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse
) {
    if (req.method === 'POST') {
        // Create a new game instance
        res.json(await NodeAppInstance.createGameInstance());
    } else if (req.method === 'PUT') {
        const gameInstance = req.body;
        if (gameInstance) {
            NodeAppInstance.updateGameInstance(gameInstance);
            res.json({ message: 'Game updated' });
        } else {
            res.status(400).json({ error: 'Missing gameInstance' });
        }
    }
}
