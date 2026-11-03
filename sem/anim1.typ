#import "canvas.typ": *


#let verts = (
  V(-40, -140, off: (0, -2)), // 1
  V(40, -140, off: (2, -2)), // 2
  V(80, -70), // 3
  V(80, 0, off: (5, -15)), // 4
  V(160, 0, off: (0, -5)), // 5
  V(220, 0), // 6
  V(160, 70, off: (5, 3)), // 7
  V(120, 140, off: (2, 2)), // 8
  V(40, 140, off: (-2, 2)), // 9
  V(0, 70, off: (-5, 0)), // 10
  V(0, 0, off: (5, 5)), // 11
  V(-70, 60, off: (0, 5)), // 12
  V(-160, 65, off: (0, 5)), // 13
  V(-200, 0, off: (-5, 0)), // 14
  V(-220, -80, off: (-5, 5)), // 15
  V(-120, -140), // 16
  // interior vertices: hidden until a C reveals them
  V(-80, -70, vis: false, off: (-8, 4)), // 17
  V(0, -70, vis: false, off: (-4, 4)), // 18
  V(80, 70, vis: false, off: (-5, -4)), // 19
  V(-100, 0, vis: false, off: (9, -4)), // 20
  V(-150, -70, vis: false, off: (-13, 13)), // 21
)
#let tris = (
  T(16, 1, 17),
  T(17, 1, 18),
  T(18, 1, 2),
  T(18, 2, 3),
  T(18, 3, 4),
  T(18, 4, 11),
  T(10, 11, 4),
  T(10, 4, 19),
  T(19, 4, 5),
  T(19, 5, 7),
  T(7, 5, 6),
  T(19, 7, 8),
  T(19, 8, 9),
  T(19, 9, 10),
  T(17, 18, 11),
  T(17, 11, 20),
  T(20, 11, 12),
  T(20, 12, 13),
  T(20, 13, 14),
  T(20, 14, 21),
  T(21, 14, 15),
  T(21, 15, 16),
  T(21, 16, 17),
  T(21, 17, 20),
)

#let comp = anim(
  def-border(clr: black, w: 3pt),
  def-stack(pos: (300, -40), n: 3, w: 70, h: 30),
  def-hist(pos: (-400, 120)),
  ..verts,
  ..tris,
  next, // blank mesh, empty stack, empty history

  //  1 C
  gate(16, 1),
  clers(16, 1, 17, "C"),
  add-h("C"),
  show-v(17),
  next,
  //  2 C
  del-t(16, 1, 17),
  gate(17, 1),
  clers(17, 1, 18, "C"),
  add-h("C"),
  show-v(18),
  next,
  //  3 R
  del-t(17, 1, 18),
  gate(18, 1),
  clers(18, 1, 2, "R"),
  add-h("R"),
  next,
  //  4 R
  del-t(18, 1, 2),
  gate(18, 2),
  clers(18, 2, 3, "R"),
  add-h("R"),
  next,
  //  5 R
  del-t(18, 2, 3),
  gate(18, 3),
  clers(18, 3, 4, "R"),
  add-h("R"),
  next,
  //  6 S  (pushes 18-11)
  del-t(18, 3, 4),
  gate(18, 4),
  clers(18, 4, 11, "S"),
  add-h("S"),
  push(18, 11),
  next,
  //  7 L
  del-t(18, 4, 11),
  gate(11, 4),
  clers(10, 11, 4, "L"),
  add-h("L"),
  next,
  //  8 C
  del-t(10, 11, 4),
  gate(10, 4),
  clers(10, 4, 19, "C"),
  add-h("C"),
  show-v(19),
  next,
  //  9 R
  del-t(10, 4, 19),
  gate(19, 4),
  clers(19, 4, 5, "R"),
  add-h("R"),
  next,
  // 10 S  (pushes 19-7)
  del-t(19, 4, 5),
  gate(19, 5),
  clers(19, 5, 7, "S"),
  add-h("S"),
  push(19, 7),
  next,
  // 11 E
  del-t(19, 5, 7),
  gate(7, 5),
  clers(7, 5, 6, "E"),
  add-h("E"),
  next,
  // 12 R  (E ended the branch -> pop 19-7, gate moves there)
  del-t(7, 5, 6),
  pop(),
  gate(19, 7),
  clers(19, 7, 8, "R"),
  add-h("R"),
  next,
  // 13 R
  del-t(19, 7, 8),
  gate(19, 8),
  clers(19, 8, 9, "R"),
  add-h("R"),
  next,
  // 14 E
  del-t(19, 8, 9),
  gate(19, 9),
  clers(19, 9, 10, "E"),
  add-h("E"),
  next,
  // 15 L  (pop 18-11)
  del-t(19, 9, 10),
  pop(),
  gate(18, 11),
  clers(17, 18, 11, "L"),
  add-h("L"),
  next,
  // 16 C
  del-t(17, 18, 11),
  gate(17, 11),
  clers(17, 11, 20, "C"),
  add-h("C"),
  show-v(20),
  next,
  // 17 R
  del-t(17, 11, 20),
  gate(20, 11),
  clers(20, 11, 12, "R"),
  add-h("R"),
  next,
  // 18 R
  del-t(20, 11, 12),
  gate(20, 12),
  clers(20, 12, 13, "R"),
  add-h("R"),
  next,
  // 19 R
  del-t(20, 12, 13),
  gate(20, 13),
  clers(20, 13, 14, "R"),
  add-h("R"),
  next,
  // 20 C
  del-t(20, 13, 14),
  gate(20, 14),
  clers(20, 14, 21, "C"),
  add-h("C"),
  show-v(21),
  next,
  // 21 R
  del-t(20, 14, 21),
  gate(21, 14),
  clers(21, 14, 15, "R"),
  add-h("R"),
  next,
  // 22 R
  del-t(21, 14, 15),
  gate(21, 15),
  clers(21, 15, 16, "R"),
  add-h("R"),
  next,
  // 23 R
  del-t(21, 15, 16),
  gate(21, 16),
  clers(21, 16, 17, "R"),
  add-h("R"),
  next,
  // 24 E
  del-t(21, 16, 17),
  gate(21, 17),
  clers(21, 17, 20, "E"),
  add-h("E"),
  next,
  // done
  del-t(21, 17, 20),
  no-gate,
)
