import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'

interface GameEndedProps {
    roundInfo: CurrentGameState
}


export default function GameEnded({ roundInfo }: GameEndedProps) {
    return (
        <h1 className="text-4xl">
            Game Ended
        </h1>
    )
}
