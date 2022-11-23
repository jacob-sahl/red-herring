import { NextResponse } from 'next/server'
import type { NextRequest } from 'next/server'

export function middleware(request: NextRequest) {   
    const { headers } = request
    const apiKey = headers
        .get('x-api-key')
        ?.trim()

    if (apiKey !== process.env.ADMIN_API_KEY) {
        const url = request.nextUrl.clone()
        url.pathname = '/api/unauthorized'
        return NextResponse.redirect(url)
    } else {
        return NextResponse.next()
    }
}

export const config = {
  matcher: '/api/admin/:path*',
}
