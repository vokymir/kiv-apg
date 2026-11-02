#import "@preview/touying:0.7.4": *
#import themes.university: *

#import "@preview/numbly:0.1.0": numbly

#import "./canvas.typ": *

#show: university-theme.with(
  aspect-ratio: "16-9",
  config-info(
    title: [Edgebreaker],
    subtitle: [Komprese trojúhelníkových sítí],
    author: [Jakub Vokoun],
    date: datetime.today(),
    institution: [Fakulta aplikovaných věd ZČU],
    contact: [#link("mailto:javok@students.zcu.cz")],
    // logo: image("./fav_logo/FAV_cmyk.pdf"),
  ),
  config-colors(
    primary: rgb(201 , 151 , 0),
    secondary: rgb(187 , 189, 199),
    tertiary: rgb("#000"),
    neutral-lightest: rgb("#ffffff"),
    neutral-darkest: rgb("#000000"),
  )
)


#set heading(numbering: numbly("{1}.", default: "1.1"))

#title-slide(logo: image("./fav_logo/FAV_cmyk.pdf",width:20%))

= Motivace

== .obj

== Paměťová náročnost

= Komprese

== Pravidla

== Animace


= Dekomprese

== Možnosti

== Pravidla

== Animace


// 1-based helpers (the figure numbers vertices 1..21)
#let tri1(a, b, c, ..args) = tri(a - 1, b - 1, c - 1, ..args)

#let mesh = make-mesh(
vertices: (
  v(-40pt, -140pt),   //  1
  v(40pt, -140pt),    //  2
  v(80pt, -70pt),    //  3
  v(80pt, 0pt),       //  4   y = 0
  v(160pt, 0pt),      //  5   y = 0
  v(220pt, 0pt),      //  6   y = 0
  v(160pt, 70pt),     //  7
  v(120pt, 140pt),    //  8
  v(40pt, 140pt),     //  9
  v(0pt, 70pt),       // 10   x = 0
  v(0pt, 0pt),        // 11   x = 0, y = 0
  v(-70pt, 60pt),     // 12
  v(-160pt, 65pt),    // 13
  v(-200pt, 0pt),     // 14   y = 0
  v(-220pt, -80pt),   // 15
  v(-120pt, -140pt),  // 16
  v(-80pt, -70pt),    // 17
  v(0pt, -70pt),      // 18   x = 0
  v(80pt, 70pt),      // 19
  v(-100pt, 0pt),     // 20   y = 0
  v(-150pt, -70pt),   // 21
),
  // listed in processing order: triangle k is removed by symbol k+1
  triangles: (
    tri1(16, 1, 17),    //  1 C
    tri1(17, 1, 18),    //  2 C
    tri1(18, 1, 2),     //  3 R
    tri1(18, 2, 3),     //  4 R
    tri1(18, 3, 4),     //  5 R
    tri1(18, 4, 11),    //  6 S  (pushes gate 18-11)
    tri1(10, 11, 4),    //  7 L
    tri1(10, 4, 19),    //  8 C
    tri1(19, 4, 5),     //  9 R
    tri1(19, 5, 7),     // 10 S  (pushes gate 19-7)
    tri1(7, 5, 6),      // 11 E
    tri1(19, 7, 8),     // 12 R  (pop 19-7)
    tri1(19, 8, 9),     // 13 R
    tri1(19, 9, 10),    // 14 E
    tri1(17, 18, 11),   // 15 L  (pop 18-11)
    tri1(17, 11, 20),   // 16 C
    tri1(20, 11, 12),   // 17 R
    tri1(20, 12, 13),   // 18 R
    tri1(20, 13, 14),   // 19 R
    tri1(20, 14, 21),   // 20 C
    tri1(21, 14, 15),   // 21 R
    tri1(21, 15, 16),   // 22 R
    tri1(21, 16, 17),   // 23 R
    tri1(21, 17, 20),   // 24 E
  ),
  edges: (),
)

// (gate, symbol), vertex numbers as in the figure (1-based)
#let history = (
 ((16, 1), "C"),
 ((17, 1), "C"),
 ((18, 1), "R"),
 ((18, 2), "R"),

 ((18, 3), "R"),
 ((18, 4), "S"),
 ((11, 4), "L"),
 ((10, 4), "C"),

 ((19, 4), "R"),
 ((19, 5), "S"),
 ((7, 5),  "E"),
 ((19, 7), "R"),

 ((19, 8), "R"),
 ((19, 9), "E"),
 ((18, 11), "L"),
 ((17, 11), "C"),

 ((20, 11), "R"),
 ((20, 12), "R"),
 ((20, 13), "R"),
 ((20, 14), "C"),

 ((21, 14), "R"),
 ((21, 15), "R"),
 ((21, 16), "R"),
 ((21, 17), "E"),
)

#let states = eb-run(
  mesh,
  history.map(((g, sym)) => (g.map(k => k - 1), sym)),   
)  

== Edgebreaker


#figure(
  alternatives(..states.map(m => draw(m, size: (80%, 80%), label-base: 1))),
  caption: [animace]
)
