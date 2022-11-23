import type { NextApiRequest, NextApiResponse } from 'next'

export default async function handler(
    req: NextApiRequest,
    res: NextApiResponse
    ) {
    const { key } = req.query
    
    if (key !== process.env.ADMIN_API_KEY) {
        res.status(401).json({ error: 'Invalid key' })
        return
    }
    
    res.status(200).json({ message: 'Valid key' })
    }