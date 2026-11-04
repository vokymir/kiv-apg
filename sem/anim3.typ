#import "canvas.typ": *
#import "anim1.typ"


#let decomp = anim(
  def-border(clr: black, w: 3pt),
  def-stack(pos: (300, -40), n: 3, w: 70, h: 30),
  def-hist(pos: (-400, 120)),
  ..anim1.verts, // same vertices as in compression
  ..range(1, 22).map(hide-v), // nothing decoded yet
  set-h("CCRRRSLCRSERRELCRRRCRRRE"),
  next,
  ..range(1, 22).map(show-v), // nothing decoded yet
  next,
  highlight-h(0),
  gate(16, 1),
  T(16, 1, 17, clr: palette.C, label: "C"),
  next,
  // remove-h(),
  highlight-h(1),
  next,
  // … continue the same way
)
