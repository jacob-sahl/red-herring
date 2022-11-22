import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'

interface DectectiveHintProps {
    roundInfo: CurrentGameState
}


export default function DectectiveHint({ roundInfo }: DectectiveHintProps) {
    return (
        <div>
            <p>You are the detective!.</p>
            <p>Follow the instructions on the screen to complete the round.</p>
        </div>
    )
}
