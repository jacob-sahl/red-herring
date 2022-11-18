import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'

interface DectectiveHintProps {
    roundInfo: CurrentGameState
}


export default function DectectiveHint({ roundInfo }: DectectiveHintProps) {
    return (
        <div>
            Follow the instructions on the screen to complete the round.
        </div>
    )
}
