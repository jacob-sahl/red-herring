import type { NextApiRequest, NextApiResponse } from 'next'
import { CurrentGameState } from '../../../../firebase/modals/round';
import NodeAppInstance from '../../../../firebase/nodeApp';

export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse
) {
    const { gameId } = req.query;
    const { session, playerId } = req.body;
    if (NodeAppInstance.validatePlayer(gameId as string, Number(playerId), session)) {
        res.json(await NodeAppInstance.getPlayerRoundInfo(gameId as string, Number(playerId)) as CurrentGameState);
    } else {
        res.status(401).json({ message: 'Unauthorized' });
    }
}
