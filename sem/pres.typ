#import "@preview/touying:0.8.0": *
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
    primary: rgb(201, 151, 0),
    secondary: rgb(187, 189, 199),
    tertiary: rgb("#000"),
    neutral-lightest: rgb("#ffffff"),
    neutral-darkest: rgb("#000000"),
  ),
)


#set heading(numbering: numbly("{1}.", default: "1.1"))

#title-slide(logo: image("./fav_logo/FAV_cmyk.pdf", width: 20%))

= Motivace


== Dobový kontext

#import "timeline.typ"
#import "timeline.typ": ev, c-cult, c-tech, c-born, c-pol


#let src = (
  nagano: [#link("https://en.wikipedia.org/w/index.php?curid=14328465")[Wikipedia, _fair use_]],
  win98: [#link("https://i1.sndcdn.com/artworks-TFheyNniW28o9wre-ddA99Q-t1080x1080.jpg")[SoundCloud (artwork)]],
  google: [#link("https://logodix.com/logo/55299.png")[logodix.com]],
  nato: [#link("https://www.nato.int/content/experience-fragments/nato/site/header/english/master/_jcr_content/root/c06_main_header_copy/logo.coreimg.png/1789636393643/nato.png")[nato.int]],
  spongebob: [#link("https://media.craiyon.com/2025-04-21/FWs1FwNsS3WCDomSej0k8w.webp")[craiyon.com]],
  putin: [Kremlin.ru, #link("https://creativecommons.org/licenses/by/4.0/")[CC BY 4.0], #link("https://commons.wikimedia.org/w/index.php?curid=163363642")[Wikimedia Commons]],
  duda: [Gov.pl, #link("https://creativecommons.org/licenses/by/3.0/pl/")[CC BY 3.0 PL], #link("https://commons.wikimedia.org/w/index.php?curid=128272581")[Wikimedia Commons]],
  mrbeast: [#link("https://yt3.ggpht.com/nxYrc_1_2f77DoBadyxMTmv7ZpRZapHR5jbuYe7PlPd5cIRJxtNNEYyOC0ZsxaDyJJzXrnJiuDE=s800-c-k-c0x00ffffff-no-rj")[YouTube (avatar kanálu)]],
  halflife: [#link("https://valvearchive.com/archive/Half-Life/Half-Life/Art/Box%20Art/front2.jpg")[Valve, valvearchive.com]; #link("https://en.wikipedia.org/w/index.php?curid=13514455")[Wikipedia _fair use_]],
  futurama: [#link("https://image.pmgstatic.com/cache/resized/w663/files/images/film/posters/168/087/168087719_geyysj.png")[ČSFD (plakát)]],
  napster: [Jusore, GPLv3, #link("https://commons.wikimedia.org/w/index.php?curid=169974746")[Wikimedia Commons]],
)

#let timeline-events = (
  ev(1998, 2, "up", 2, [Nagano], image("images/nagano.svg", fit: "contain"), src: src.nagano, ih: 55pt, clr: c-cult),
  ev(1998, 5, "up", 1, [Windows 98], image("images/win98.jpg"), src: src.win98, w: 100pt, ih: 70pt, clr: c-tech),
  ev(1998, 9, "up", 2, [Google], image("images/google.png"), src: src.google, clr: c-tech),
  ev(1999, 3, "up", 2, [NATO], image("images/nato.png"), src: src.nato, al: center, clr: c-pol),
  ev(1999, 5, "up", 1, [SpongeBob], image("images/spongebob.webp"), src: src.spongebob, ih: 70pt, clr: c-cult),
  ev(1999, 12, "up", 1, [Putin], image("images/putin.jpg"), src: src.putin, focus: 0, ih: 110pt, al: right, clr: c-pol),
  ev(1998, 4, "down", 1, [Duda], image("images/duda.jpg"), src: src.duda, ih: 120pt, al: right, clr: c-born),
  ev(1998, 5, "down", 2, [MrBeast], image("images/mrbeast.jpg"), src: src.mrbeast, ih: 60pt, clr: c-born),
  ev(1998, 11, "down", 1, [Half-Life], image("images/half-life.jpg"), src:
  src.halflife, ih: 90pt, al: right, clr: c-cult),
  ev(1999, 3, "down", 2, [Futurama], image("images/futurama.png"), src: src.futurama, clr: c-cult),
  ev(1999, 6, "down", 1, [Napster], image("images/napster.svg", fit: "contain"), src: src.napster, clr: c-tech),
)

#figure(
  timeline.timeline(
    mid: [1/1999 \ Edgebreaker \ #text(size: 0.8em, weight: "regular")[J. Rossignac, IEEE TVCG]],
    timeline-events,
    spans: (
      (t0: 17, t1: 24, lvl: 0.40, body: [nová slova: _carbon footprint_, _blog_, _texting_]),
    ),
  ),
  caption: [Dobový kontext vzniku algoritmu Edgebreaker.],
)

// nad čím jarek přemýšlel?
#slides-only()[
  #figure(
    image("images/rossignac.jpg", height: 70%),
    caption: [Jarek Rossignac.]
  )
  zdroj: #link("http://kelvinkuo.com/wp-content/uploads/2010/03/KKK_6258.jpg")
]

#focus-slide()[

  #align(center)[disk vs RAM]
]


= Definice problému

== Mesh: geometrie + konektivita

#import "mesh-def.typ"

#figure(
  alternatives(..mesh-def.comp.map(draw.with(size: (80%, 80%)))),
  caption: [Trojúhelníková síť (mesh).],
)

== Jak schovat informaci?

// #rect(height: 1%, stroke: none)
#columns(2)[
#pause
pořadí vertexů

  [V1 V2 V3]\
#pause
  [V1 V3 V2]\
  [V2 V1 V3]\
  [V3 V1 V2]\
  [V2 V3 V1]\
  [V3 V2 V1]

#colbreak()

#pause
CLERS řetězec

#pause
CLRSLRCLEE
]

== Klasifikace

#import "CLERS.typ"

#figure(
  rect(height: 90%, stroke: none)[
    #grid(
  columns: 3,
  CLERS.C,
  CLERS.L,
  CLERS.R,
)

    #grid(
  columns: 2,
  CLERS.S,
  CLERS.E,
)
],

caption: [Základní CLERS operace.]
)

= Komprese

== Omezení

Mesh musí být *orientovatelný manifold* homeomorfní s koulí/kruhem.

Lehce upravená verze algoritmu zvládne i _díry_ a _handly_, obecně meshe s
vyšším genusem.

== Animace

#import "anim1.typ"

#figure(
  alternatives(..anim1.comp.map(draw.with(size: (80%, 80%)))),
  caption: [Jednoduchá komprese.],
 
)

== Výsledný soubor

Geometrie vrcholů: #rect()[1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21]

CLERS řetězec: #rect()[CCRRRSLCRSERRELCRRRCRRRE]

#pause
Kódování: C = 0, S = 100, R = 101, L = 110, E = 111

#pause
(CLERS) 0110111101100 bitové pole: #rect()[#text(size: 0.95em)[00101101 10110011
00101100 11110110 11111100 10110110 10101101 101111]]


= Dekomprese

== Možnosti

Existuje víc algoritmů, implementuji spirale-reversi a wrap&zip.

Demonstrováno na wrap&zip, je intuitivnější.

== Animace

#import "anim3.typ"

#figure(
  alternatives(..anim3.decomp.map(draw.with(size: (80%, 80%)))),
  caption: [Dekomprese.],
)

= Vyšší genus

== Řešení

- obecnější S
- navíc ukládání topologie

#pause
=== Díry

objevení díry => push topologie s délkou díry

#pause
=== Handly

následuje S => push topologie se "zipem"

== Animace

#import "anim2.typ"

#figure(
  alternatives(..anim2.comp.map(draw.with(size: (80%, 80%)))),
  caption: [Složitější komprese.],
)


= Zdroje

#let figure-outline() = context {
  let seen = ()
  let items = ()
  for f in query(figure) {
    if f.caption == none { continue }
    let id = repr(f.caption.body)          // same caption = same figure
    if id not in seen {
      seen.push(id)
      items.push(f)                        // first occurrence = first subslide
    }
  }
  for (i, f) in items.enumerate() {
    let loc = f.location()
    link(loc)[
      Figure #(i + 1): #f.caption.body
      #box(width: 1fr, repeat[.])
      // touying shows THE slide
      #utils.slide-counter.at(loc).first()
    ]
    linebreak()
  }
}


== Seznam _figures_

#figure-outline()

== Obrázky (dobový kontext)

#timeline.credits(timeline-events, size: 1em)

== Reference

#set text(size: 0.6em)

Originální paper @rossignac1999edgebreaker
Corner table @rossignac2003corner
Významné vylepšení @lewiner2004efficient
Wrap & Zip @rossignac1999wrapzipreport
Inspirace animací @eb-slides

#bibliography("./refs.bib", title: none, style: "ieee")

