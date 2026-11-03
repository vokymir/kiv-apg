#import "canvas.typ": *
#import "anim1.typ"


#let decomp = anim(
  def-border(clr: black, w: 3pt),
  def-stack(pos: (275, -40), n: 3, w: 70, h: 30),
  def-hist(pos: (-300, 140)),
  ..anim1.verts, // same vertices as in compression
  ..range(1, 22).map(hide-v), // nothing decoded yet
  set-h("CCRRRSLCRSERRELCRRRCRRRE"),
  next,
  highlight-h(), // head = last symbol (E)
  next,
  T(21, 17, 20, clr: palette.E, label: "E"),
  show-v(21),
  show-v(17),
  show-v(20),
  gate(21, 17),
  next,
  remove-h(),
  highlight-h(), // consume it, new head
  next,
  // … continue the same way
)
