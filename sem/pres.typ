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

#include "timeline.typ"
nagano:
By [1], Fair use, https://en.wikipedia.org/w/index.php?curid=14328465
win98:
https://i1.sndcdn.com/artworks-TFheyNniW28o9wre-ddA99Q-t1080x1080.jpg
google:
https://logodix.com/logo/55299.png
nato:
https://www.nato.int/content/experience-fragments/nato/site/header/english/master/_jcr_content/root/c06_main_header_copy/logo.coreimg.png/1789636393643/nato.png
sponge bob:
https://media.craiyon.com/2025-04-21/FWs1FwNsS3WCDomSej0k8w.webp
putin:
Autor: Kremlin.ru, CC BY 4.0, https://commons.wikimedia.org/w/index.php?curid=163363642
duda:
Autor: Gov.pl, CC BY 3.0 pl, https://commons.wikimedia.org/w/index.php?curid=128272581
mrbeast:
https://yt3.ggpht.com/nxYrc_1_2f77DoBadyxMTmv7ZpRZapHR5jbuYe7PlPd5cIRJxtNNEYyOC0ZsxaDyJJzXrnJiuDE=s800-c-k-c0x00ffffff-no-rj
half-life:
By May be found at the following website: https://valvearchive.com/archive/Half-Life/Half-Life/Art/Box%20Art/front2.jpg, Fair use, https://en.wikipedia.org/w/index.php?curid=13514455
futurama: (csfd)
https://image.pmgstatic.com/cache/resized/w663/files/images/film/posters/168/087/168087719_geyysj.png
napster:
Autor: Jusore – Vlastní dílo, GPLv3, https://commons.wikimedia.org/w/index.php?curid=169974746

== Dělení

#pause
=== Geometrie

$ forall v in V: $

#pause

#alternatives[
  $ v = (x, y, z) $
][
  $ v = (x, y, z, #text(red)[$n, "uv", dots$]) $
]

#pause
=== Konektivita

#let eqs = (
  $ forall e in E: e = (e_i, e_j) $,
  $ forall t in T: t = (v_i, v_j, v_k) $,
  $ forall c in 3T: c = (c_("op")) $,
)

#pause

#alternatives(
  ..range(1, eqs.len() + 1).map(n => eqs.slice(0, n).join()),
  $ V : 2F : 3E $,
)


= Komprese

== Pravidla

// ...CLERS TODO


// navíc manifold & orientovaný

// díry a handly zvládneme


== Animace

#import "anim1.typ"

#figure(
  alternatives(..anim1.comp.map(draw.with(size: (80%, 80%)))),
  caption: [Jednoduchá komprese.],
)

== Vyšší genus?

#import "anim2.typ"

#figure(
  alternatives(..anim2.comp.map(draw.with(size: (80%, 80%)))),
  caption: [Komprese díry.],
)


= Dekomprese

== Možnosti

//TODO: basic, spirale-reversi, wrap&zip


== Pravidla

// TODO: wrap&zip

// TODO: včetně genusu! ale ten nebude animován

== Animace

// TODO: animace wrap&zip

#import "anim3.typ"

#figure(
  alternatives(..anim3.decomp.map(draw.with(size: (80%, 80%)))),
  caption: [Dekomprese.],
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
      Obrázek #(i + 1): #f.caption.body
      #box(width: 1fr, repeat[.])
      // touying shows THE slide
      #utils.slide-counter.at(loc).first()
    ]
    linebreak()
  }
}

== Obrázky

#figure-outline()

== Reference
#set text(size: 0.6em)

Originální paper @rossignac1999edgebreaker
Corner table @rossignac2003corner
Významné vylepšení @lewiner2004efficient
Wrap & Zip @rossignac1999wrapzipreport
Inspirace animací @eb-slides

#bibliography("refs.bib", title: none, style: "ieee")
