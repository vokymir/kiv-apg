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

highlight-h(0),
gate(1, 16),
show-v(1, label: [F1]),
show-v(16, label: [F3]),
show-v(17, label: [F2]),
T(1, 17, 16, clr: palette.C, label: "C"),
next,

highlight-h(1),
gate(1, 17),
show-v(18, label: [F4]),
T(1, 18, 17, clr: palette.C, label: "C"),
next,

highlight-h(2),
gate(1, 18),
show-v(2, label: [F5]),
T(1, 2, 18, clr: palette.R, label: "R"),
next,

highlight-h(3),
gate(2, 18),
show-v(3, label: [F6]),
T(2, 3, 18, clr: palette.R, label: "R"),
next,

highlight-h(4),
gate(3, 18),
show-v(4, label: [F7]),
T(3, 4, 18, clr: palette.R, label: "R"),
next,

highlight-h(5),
gate(4, 18),
show-v(11, label: [F8]),
T(4, 11, 18, clr: palette.S, label: "S"),
next,

push(11, 18),
highlight-h(6),
gate(4, 11),
show-v(10, label: [F9]),
T(4, 10, 11, clr: palette.L, label: "L"),
next,

highlight-h(7),
gate(4, 10),
show-v(19, label: [F10], off: (-10, -4)),
T(4, 19, 10, clr: palette.C, label: "C"),
next,

highlight-h(8),
gate(4, 19),
show-v(5, label: [F11]),
T(4, 5, 19, clr: palette.R, label: "R"),
next,

highlight-h(9),
gate(5, 19),
show-v(7, label: [F12]),
T(5, 7, 19, clr: palette.S, label: "S"),
next,

highlight-h(10),
push(7, 19),
gate(5, 7),
show-v(6, label: [F13], off: (0, -8)),
T(5, 6, 7, clr: palette.E, label: "E"),
next,

highlight-h(11),
pop(),
gate(7, 19),
show-v(8, label: [F14], off: (15, 0)),
T(7, 8, 19, clr: palette.R, label: "R"),
next,

highlight-h(12),
gate(8, 19),
show-v(9, label: [F15], off: (0, 8)),
T(8, 9, 19, clr: palette.R, label: "R"),
next,

highlight-h(13),
gate(9, 19),
V(0, 95, label: [F16], off: (-10, 0)), // 22
// show-v(10, label: [F16], off: (0, 8)),
// T(9, 10, 19, clr: palette.E, label: "E"),
T(9, 22, 19, clr: palette.E, label: "E"),
next,

hide-v(22),
del-t(9, 22, 19),
show-v(10, label: [F16 -> F9], off: (-30, 8)),
T(9, 10, 19, clr: palette.E, label: "E"),
next,

show-v(10, label: [F9], off: (0, 8)),
highlight-h(14),
pop(),
gate(11, 18),
T(11, 17, 18, clr: palette.L, label: "L"),
next,

highlight-h(15),
gate(11, 17),
show-v(20, label: [F16], off: (12, -5)),
T(11, 17, 20, clr: palette.C, label: "C"),
next,

highlight-h(16),
gate(11, 20),
show-v(12, label: [F17], off: (12, 0)),
T(11, 12, 20, clr: palette.R, label: "R"),
next,

highlight-h(17),
gate(12, 20),
show-v(13, label: [F18], off: (0, 10)),
T(12, 13, 20, clr: palette.R, label: "R"),
next,

highlight-h(18),
gate(13, 20),
show-v(14, label: [F19], off: (-10, 0)),
T(13, 14, 20, clr: palette.R, label: "R"),
next,

highlight-h(19),
gate(14, 20),
show-v(21, label: [F20], off: (-15, 12)),
T(14, 21, 20, clr: palette.C, label: "C"),
next,

highlight-h(20),
gate(14, 21),
show-v(15, label: [F21], off: (-10, 0)),
T(14, 15, 21, clr: palette.R, label: "R"),
next,

highlight-h(21),
gate(15, 21),
// V23 TODO
V(-160, -140, label: [F22], off: (0, -10)),
T(15,23,21, clr: palette.R, label: "R"),
// T(15, 16, 21, clr: palette.R, label: "R"),
next,

highlight-h(22),
// V24 TODO
V(-120,-100, label: [F23], off: (-10,0)),
T(21, 24, 23, clr: palette.R, label: "R"),
gate(23, 21),
// T(16, 17, 21, clr: palette.R, label: "R"),
next,

highlight-h(23),
gate(24, 21),
// V25 TODO
V(-100,-30, label: [F24], off: (0,10)),
T(24,21,25, clr: palette.E, label: "E"),
// T(17, 20, 21, clr: palette.E, label: "E"),
next,

no-gate,
del-t(24,21,25),
T(20,21,24, clr: palette.E, label: "E"),
hide-v(25),
next,

del-t(20,21,24),
T(17,20,21, clr: palette.E, label: "E"),
del-t(21,24,23),
hide-v(24),
T(21,23,17, clr: palette.R, label: "R"),
next,

del-t(21,23,17),
T(16, 17, 21, clr: palette.R, label: "R"),
T(15, 16, 21, clr: palette.R, label: "R"),
del-t(15,23,21),
hide-v(23),
next,

no-highlight,
)
