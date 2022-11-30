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
            ctx.drawImage(imgRef.current!, 0, 0, 375, 565.5);
            ctx.font = "20px EB Garamond";
            ctx.fillStyle = "black";
            ctx.textAlign = "center";
            wrapText(ctx, "CLUE", 187, 85, 180, 20);
            wrapText(ctx, roundInfo.currentInformantCard?.clue!, 187, 105, 300, 15);
            wrapText(ctx, "GOAL", 187, 400, 180, 20);
            wrapText(ctx, roundInfo.currentInformantCard?.secretGoal!, 187, 420, 300, 15);

    };
    useEffect(() => {
        const canvas = canvasRef.current;
        const context = canvas!.getContext('2d');
        draw(context!)
    }, [draw])
    return (
        <div className="m-auto" >
            <canvas ref={canvasRef} width={375} height={565.5} className="scale-100 m-auto" />
            <img src={CardFrameImg.src} alt="Informant Card" hidden ref={imgRef} />
        </div>
    )
}
