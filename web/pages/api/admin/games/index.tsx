import type { NextApiRequest, NextApiResponse } from 'next'
import NodeAppInstance from '../../../../firebase/nodeApp';

export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse
) {
    if (req.method === 'POST') {
        // Create a new game instance
        res.json(await NodeAppInstance.createGameInstance());
    }
}
