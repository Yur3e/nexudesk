interface BrandProps {
  inverted?: boolean
}

export function Brand({ inverted = false }: BrandProps) {
  return (
    <span className={`brand-lockup${inverted ? ' brand-lockup-inverted' : ''}`}>
      <span className="brand-mark" aria-hidden="true">
        <svg viewBox="0 0 40 40" fill="none">
          <path d="M10 13.5C10 10.46 12.46 8 15.5 8h9C27.54 8 30 10.46 30 13.5v7C30 23.54 27.54 26 24.5 26H20l-5.75 5.1c-.64.57-1.65.11-1.65-.75V26.8A5.5 5.5 0 0 1 10 22V13.5Z" fill="currentColor" />
          <path d="M15.5 17h9M15.5 21h5" stroke="white" strokeWidth="2.1" strokeLinecap="round" />
        </svg>
      </span>
      <span className="brand-word">nexo<span>desk</span></span>
    </span>
  )
}
