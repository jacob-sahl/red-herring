import React, { ReactComponentElement } from 'react'
import { CurrentGameState } from '../firebase/modals/round'

interface DectectiveHintProps {
    roundInfo: CurrentGameState
}


export default function DectectiveHint({ roundInfo }: DectectiveHintProps) {
    return (
        <div>
            <h1 className='text-4xl'>You are the detective!</h1>
            <p className='text-xl'>Follow the instructions on the screen to complete the round.</p>
        </div>
    )
}
