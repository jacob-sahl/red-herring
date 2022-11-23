import React, { ReactComponentElement, useEffect } from 'react'
import Image from 'next/image'
import { CurrentGameState } from '../firebase/modals/round'
import CardFrameImg from '../assets/CardFrame.png'
interface InformantHintProps {
    roundInfo: CurrentGameState
}

function wrapText(context: any, text: string, x: number, y: number, maxWidth: number, lineHeight: number) {
    let words = text.split(' ');
    let line = '';

    for(let n = 0; n < words.length; n++) {
      let testLine = line + words[n] + ' ';
      let metrics = context.measureText(testLine);
      let testWidth = metrics.width;
      if (testWidth > maxWidth && n > 0) {
        context.fillText(line, x, y);
        line = words[n] + ' ';
        y += lineHeight;
      }
      else {
        line = testLine;
      }
    }
    context.fillText(line, x, y);
  }

export default function InformantHint({ roundInfo }: InformantHintProps) {
    const canvasRef = React.useRef<HTMLCanvasElement>(null);
    const imgRef = React.useRef<HTMLImageElement>(null);
    const draw = (ctx: CanvasRenderingContext2D) => {
            ctx.drawImage(imgRef.current!, 0, 0, 251, 377);
            ctx.font = "15px Sans-Serif";
            ctx.fillStyle = "black";
            ctx.textAlign = "center";
            wrapText(ctx, "CLUE", 125, 65, 180, 20);
            wrapText(ctx, roundInfo.currentInformantCard?.clue!, 125, 85, 180, 15);
            wrapText(ctx, "GOAL", 125, 270, 180, 20);
            wrapText(ctx, roundInfo.currentInformantCard?.secretGoal!, 125, 290, 200, 15);
    };
    useEffect(() => {
        const canvas = canvasRef.current;
        const context = canvas!.getContext('2d');
        draw(context!)
    }, [draw])
    return (
        <div className="m-auto" >
            <canvas ref={canvasRef} width={250} height={377} className="scale-100 m-auto" />
            <img src={CardFrameImg.src} alt="Informant Card" hidden ref={imgRef} />
        </div>
    )
}
