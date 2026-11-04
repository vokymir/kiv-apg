#import "canvas.typ": palette

#let p = (..palette, VISITED: gray, UNVISITED: white)

#let centered-place(x, y, content) = context {
  let size = measure(content)

  place(
    dx: x - size.width / 2,
    dy: y - size.height / 2,
    content,
  )
}

#let arrow(a, b, clr: black, w: 1.6pt, head: 8pt) = place(layout(sz => {
  let pt = q => (q.at(0) * sz.width, q.at(1) * sz.height) // 50% -> length
  let (a, b) = (pt(a), pt(b))
  let (dx, dy) = ((b.at(0) - a.at(0)) / 1pt, (b.at(1) - a.at(1)) / 1pt)
  let l = calc.sqrt(dx * dx + dy * dy)
  let (ux, uy) = (dx / l, dy / l) // unit vector a -> b
  let base = (b.at(0) - ux * head, b.at(1) - uy * head) // centre of the head's back edge
  let tail = (b.at(0) - ux * head * 0.8, b.at(1) - uy * head * 0.8)
  place(line(start: a, end: tail, stroke: w + clr))
  place(polygon(
    fill: clr,
    stroke: none,
    b, // tip
    (base.at(0) - uy * head / 2, base.at(1) + ux * head / 2),
    (base.at(0) + uy * head / 2, base.at(1) - ux * head / 2),
  ))
}))

#let C = box(inset: (top: 30pt, rest: 10pt), box(width: 160pt, height: 120pt, {
  let stroke = black
  let v1 = (25%, 50%)
  let v2 = (75%, 50%)
  let v3 = (50%, 0%)
  let v4 = (50%, 100%)
  let v5 = (100%, 0%)
  let v6 = (0%, 0%)

  place(polygon(fill: p.UNVISITED, stroke: stroke, v1, v3, v6)) // LEFT
  place(polygon(fill: p.UNVISITED, stroke: stroke, v2, v5, v3)) // RIGHT
  place(polygon(fill: p.VISITED, stroke: stroke, v4, v2, v1)) // DOWN
  place(polygon(fill: p.C, stroke: stroke, v1, v2, v3)) // C

  // triangle letter
  centered-place(50%, 30%, text(size: 16pt, weight: "bold")[C])

  // arrows
  arrow((50%, 60%), (50%, 40%))
  arrow((57%, 32%), (70%, 17%))

  // vertex V
  centered-place(50%, 0%)[#circle(radius: 4pt, fill: white, stroke: stroke)]
  centered-place(50%, -10%, text(size: 16pt)[V])
}))

#let L = box(inset: (top: 30pt, rest: 10pt), box(width: 160pt, height: 120pt, {
  let stroke = black
  let v1 = (25%, 50%)
  let v2 = (75%, 50%)
  let v3 = (50%, 0%)
  let v4 = (50%, 100%)
  let v5 = (100%, 0%)
  let v6 = (0%, 0%)

  place(polygon(fill: p.VISITED, stroke: stroke, v1, v3, v6)) // LEFT
  place(polygon(fill: p.UNVISITED, stroke: stroke, v2, v5, v3)) // RIGHT
  place(polygon(fill: p.VISITED, stroke: stroke, v4, v2, v1)) // DOWN
  place(polygon(fill: p.L, stroke: stroke, v1, v2, v3)) // C

  // triangle letter
  centered-place(50%, 30%, text(size: 16pt, weight: "bold")[L])

  // arrows
  arrow((50%, 60%), (50%, 40%))
  arrow((57%, 32%), (70%, 17%))

  // vertex V
  centered-place(50%, 0%)[#circle(radius: 4pt, fill: black, stroke: stroke)]
  centered-place(50%, -10%, text(size: 16pt)[V])
}))

#let R = box(inset: (top: 30pt, rest: 10pt), box(width: 160pt, height: 120pt, {
  let stroke = black
  let v1 = (25%, 50%)
  let v2 = (75%, 50%)
  let v3 = (50%, 0%)
  let v4 = (50%, 100%)
  let v5 = (100%, 0%)
  let v6 = (0%, 0%)

  place(polygon(fill: p.UNVISITED, stroke: stroke, v1, v3, v6)) // LEFT
  place(polygon(fill: p.VISITED, stroke: stroke, v2, v5, v3)) // RIGHT
  place(polygon(fill: p.VISITED, stroke: stroke, v4, v2, v1)) // DOWN
  place(polygon(fill: p.R, stroke: stroke, v1, v2, v3)) // C

  // triangle letter
  centered-place(50%, 30%, text(size: 16pt, weight: "bold")[R])

  // arrows
  arrow((50%, 60%), (50%, 40%))
  arrow((43%, 32%), (30%, 17%))

  // vertex V
  centered-place(50%, 0%)[#circle(radius: 4pt, fill: black, stroke: stroke)]
  centered-place(50%, -10%, text(size: 16pt)[V])
}))

#let S = box(inset: (top: 30pt, rest: 10pt), box(width: 160pt, height: 120pt, {
  let stroke = black
  let v1 = (25%, 50%)
  let v2 = (75%, 50%)
  let v3 = (50%, 0%)
  let v4 = (50%, 100%)
  let v5 = (100%, 0%)
  let v6 = (0%, 0%)

  place(polygon(fill: p.UNVISITED, stroke: stroke, v1, v3, v6)) // LEFT
  place(polygon(fill: p.UNVISITED, stroke: stroke, v2, v5, v3)) // RIGHT
  place(polygon(fill: p.VISITED, stroke: stroke, v4, v2, v1)) // DOWN
  place(polygon(fill: p.S, stroke: stroke, v1, v2, v3)) // C

  // triangle letter
  centered-place(50%, 30%, text(size: 16pt, weight: "bold")[S])

  // arrows
  arrow((50%, 60%), (50%, 40%))
  arrow((57%, 32%), (70%, 17%))
  arrow((43%, 32%), (30%, 17%))

  // vertex V
  centered-place(50%, 0%)[#circle(radius: 4pt, fill: black, stroke: stroke)]
  centered-place(50%, -10%, text(size: 16pt)[V])
}))

#let E = box(inset: (top: 30pt, rest: 10pt), box(width: 160pt, height: 120pt, {
  let stroke = black
  let v1 = (25%, 50%)
  let v2 = (75%, 50%)
  let v3 = (50%, 0%)
  let v4 = (50%, 100%)
  let v5 = (100%, 0%)
  let v6 = (0%, 0%)

  place(polygon(fill: p.VISITED, stroke: stroke, v1, v3, v6)) // LEFT
  place(polygon(fill: p.VISITED, stroke: stroke, v2, v5, v3)) // RIGHT
  place(polygon(fill: p.VISITED, stroke: stroke, v4, v2, v1)) // DOWN
  place(polygon(fill: p.E, stroke: stroke, v1, v2, v3)) // C

  // triangle letter
  centered-place(50%, 30%, text(size: 16pt, weight: "bold")[E])

  // arrows
  arrow((50%, 60%), (50%, 40%))

  // vertex V
  centered-place(50%, 0%)[#circle(radius: 4pt, fill: black, stroke: stroke)]
  centered-place(50%, -10%, text(size: 16pt)[V])
}))
