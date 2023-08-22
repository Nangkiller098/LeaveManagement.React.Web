"use client"    
import React from 'react'
import Link from 'next/link'
import Image from 'next/image'
import logo from './dojo-logo.png'

export default function Navbar() {
  return (
    <nav>
          <Image
      src={logo} 
      width={70}
      alt="Leave Management"
      quality={100}
      placeholder='blur'
    />
        <h1>Leave Management React</h1>
        <Link href="/">Home</Link>
        <Link href="/">Apply For Leave</Link>
        <Link href="/">My Leave</Link>
        <Link href="/">Employee</Link>
        <Link href="/leavetypes">Leave Types</Link>

    </nav>

  )
}