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
  ..range(1, 17).map(show-v), // ONLY GEOMTERY
  next,

  highlight-h(0),
  gate(1, 16),
  show-v(17),
  T(1, 17, 16, clr: palette.C, label: "C"),
  next,

  highlight-h(1),
  gate(1, 17),
  show-v(18),
  T(1, 18, 17, clr: palette.C, label: "C"),
  next,

  highlight-h(2),
  gate(1, 18),
  T(1, 2, 18, clr: palette.R, label: "R"),
  next,

  highlight-h(3),
  gate(2, 18),
  T(2, 3, 18, clr: palette.R, label: "R"),
  next,

  highlight-h(4),
  gate(3, 18),
  T(3, 4, 18, clr: palette.R, label: "R"),
  next,

  highlight-h(5),
  gate(4, 18),
  T(4, 11, 18, clr: palette.S, label: "S"),
  next,

  push(11, 18),
  highlight-h(6),
  gate(4, 11),
  T(4, 10, 11, clr: palette.L, label: "L"),
  next,

  highlight-h(7),
  gate(4, 10),
  show-v(19),
  T(4, 19, 10, clr: palette.C, label: "C"),
  next,

  highlight-h(8),
  gate(4, 19),
  T(4, 5, 19, clr: palette.R, label: "R"),
  next,

  highlight-h(9),
  gate(5, 19),
  T(5, 7, 19, clr: palette.S, label: "S"),
  next,

  highlight-h(10),
  push(7, 19),
  gate(5, 7),
  T(5, 6, 7, clr: palette.E, label: "E"),
  next,

  highlight-h(11),
  pop(),
  gate(7, 19),
  T(7, 8, 19, clr: palette.R, label: "R"),
  next,

  highlight-h(12),
  gate(8, 19),
  T(8, 9, 19, clr: palette.R, label: "R"),
  next,

  highlight-h(13),
  gate(9, 19),
  T(9, 10, 19, clr: palette.E, label: "E"),
  next,

  highlight-h(14),
  pop(),
  gate(11, 18),
  T(11, 17, 18, clr: palette.L, label: "L"),
  next,

  highlight-h(15),
  gate(11, 17),
  show-v(20),
  T(11, 17, 20, clr: palette.C, label: "C"),
  next,

  highlight-h(16),
  gate(11, 20),
  T(11, 12, 20, clr: palette.R, label: "R"),
  next,

  highlight-h(17),
  gate(12, 20),
  T(12, 13, 20, clr: palette.R, label: "R"),
  next,

  highlight-h(18),
  gate(13, 20),
  T(13, 14, 20, clr: palette.R, label: "R"),
  next,

  highlight-h(19),
  gate(14, 20),
  show-v(21),
  T(14, 21, 20, clr: palette.C, label: "C"),
  next,

  highlight-h(20),
  gate(14, 21),
  T(14, 15, 21, clr: palette.R, label: "R"),
  next,

  highlight-h(21),
  gate(15, 21),
  T(15, 16, 21, clr: palette.R, label: "R"),
  next,

  highlight-h(22),
  gate(16, 21),
  T(16, 17, 21, clr: palette.R, label: "R"),
  next,

  highlight-h(23),
  gate(17, 21),
  T(17, 20, 21, clr: palette.E, label: "E"),
  next,

  no-highlight,
  no-gate,
)
