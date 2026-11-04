#import "canvas.typ": *

#let verts = (
  V(-80, -70, off: (-8, 0), label: [V1]),
  V(80, -70, off: (8, 0), label: [V2]),
  V(80, 70, off: (8, 0), label: [V3]),
  V(-80, 70, off: (-8, 0), label: [V4]),
)
#let tris = (
  T(1, 2, 3, label: [T0]),
  T(1, 3, 4, label: [T1]),
)

#let comp = anim(
  def-border(clr: black, w: 1pt),
  // def-stack(pos: (300, -40), n: 3, w: 70, h: 30),
  // def-hist(pos: (-400, 120)),
  ..verts,
  ..tris,
  next, // blank mesh, empty stack, empty history

  del-t(1, 2, 3),
  del-t(1, 3, 4),

  next,

  T(1, 2, 3, label: [T0]),
  T(1, 3, 4, label: [T1]),
  set-v(1, vis: false),
  set-v(2, vis: false),
  set-v(3, vis: false),
  set-v(4, vis: false),
)
