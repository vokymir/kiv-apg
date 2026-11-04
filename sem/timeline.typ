// ---------- timeline helpers ----------
#let tl-t(y, m) = (y - 1998) * 12 + (m - 1)      // months since 1/1998 (0..24)

// side: "up" | "down";  lvl: 1,2,.. = distance from the axis;
// al: left | right | center = which side of the cone apex the card extends to
// img: content (e.g. image("img/x.jpg")) or none -> placeholder
#let ev(
  y,
  m,
  side,
  lvl,
  title,
  img,
  al: left,
  clr: gray.darken(40%),
  src: none,
  w: auto,
  ih: auto,
  focus: none, // none = cover/centre as before;  0 = top, 0.5 = middle, 1 = bottom
  zoom: 1, // 1 = image fills card width, 1.5 = zoomed in 1.5x (centred horizontally)
) = (
  t: tl-t(y, m),
  side: side,
  lvl: lvl,
  al: al,
  clr: clr,
  src: src,
  title: title,
  date: [#m/#y],
  img: img,
  w: w,
  ih: ih,
  focus: focus,
  zoom: zoom,
)

#let timeline(
  events,
  mid: none, // centre piece (Edgebreaker)
  spans: (), // (t0:, t1:, lvl:, clr:, body:)
  width: 100%,
  H: 158pt, // half-height (axis is in the middle)
  stem0: 36pt,
  step: 62pt, // distance axis -> card: level 1 / increment per level
  card-w: 84pt,
  img-h: 46pt,
  tags: true,
  fs: 9pt,
  cone-alpha: 65%, // cone transparency
  mid-fill: rgb(201, 151, 0),
  mid-size: 12pt,
) = block(width: width, height: 2 * H, layout(sz => block(width: sz.width, height: 2 * H, {
  let events = events.sorted(key: e => e.t)
  set text(size: fs)
  let W = sz.width
  let X = t => t / 24 * W
  let stem = lvl => stem0 + (lvl - 1) * step
  // let card-dx = (e, x) => if e.al == left { x } else if e.al == right { x - card-w } else { x - card-w / 2 }
  let wid = e => if e.w == auto { card-w } else { e.w }
  let hei = e => if e.ih == auto { img-h } else { e.ih }
  let card-dx = (e, x) => {
    let w = wid(e)
    if e.al == left { x } else if e.al == right { x - w } else { x - w / 2 }
  }

  // 1) cones: apex on the axis, base = near edge of the card
  for e in events {
    let x = X(e.t)
    let dx = card-dx(e, x)
    let cw = wid(e)
    let by = if e.side == "up" { H - stem(e.lvl) } else { H + stem(e.lvl) }
    place(top + left, polygon(
      fill: e.clr.transparentize(cone-alpha),
      stroke: none,
      (x, H),
      (dx, by),
      (dx + cw, by),
    ))
  }

  // 2) axis + end years (no ticks)
  place(top + left, dy: H, line(length: W, stroke: 1.5pt))
  place(top + left, dy: H + 8pt, text(size: 1.0em, [1998]))
  place(top + right, dy: H + 8pt, text(size: 1.0em, [2000]))

  // 3) cards: rounded rectangle = header (title + date) + image
  // 3) cards
  for (i, e) in events.enumerate() {
    let dx = card-dx(e, X(e.t))
    let s = stem(e.lvl)
    let cw = wid(e) // card width
    let ch = hei(e) // image height
    let card = box(
      width: cw,
      radius: 5pt,
      clip: true,
      fill: white,
      stroke: 1.5pt + e.clr,
      stack(
        box(width: 100%, fill: e.clr, inset: (x: 4pt, y: 3pt), grid(
          columns: (1fr, auto),
          column-gutter: 3pt,
          text(fill: white, weight: "bold", e.title), text(fill: white, e.date),
        )),
        box(width: 100%, height: ch, clip: true, context {
          if e.img == none {
            align(center + horizon, text(fill: gray, [obrázek]))
          } else if e.focus == none {
            set image(width: 100%, height: ch, fit: "cover")
            e.img
          } else {
            let iw = cw * e.zoom
            let im = {
              set image(width: iw, height: auto)
              e.img
            }
            let m = measure(im)
            move(dx: -(iw - cw) / 2, dy: -calc.max(0pt, m.height - ch) * e.focus, im)
          }

          if tags {
            place(bottom + right, box(
              fill: black.transparentize(35%),
              inset: (x: 2pt, y: 1pt),
              radius: (top-left: 2pt),
              text(size: 8pt, fill: white, [#(i + 1)]),
            ))
          }
        }),
      ),
    )
    if e.side == "up" { place(bottom + left, dx: dx, dy: -(H + s), card) } else {
      place(top + left, dx: dx, dy: H + s, card)
    }
  }

  // 4) time spans (e.g. "all of 1999")
  for sp in spans {
    let (x0, x1) = (X(sp.t0), X(sp.t1))
    let y = H + stem(sp.lvl)
    // place(top + left, dx: x0, dy: y, line(length: x1 - x0, stroke: 3pt + sp.clr))
    place(top + left, dx: x0, dy: y + 5pt, box(width: x1 - x0, align(center, sp.body)))
  }

  // 5) centre piece (as before)
  if mid != none {
    place(horizon + left, dx: 50% - 75pt, box(width: 150pt, fill: mid-fill, inset: 8pt, radius: 4pt, align(center, text(
      size: mid-size,
      weight: "bold",
      mid,
    ))))
  }
})))

#let credits(events, size: 0.55em) = {
  set text(size: size)
  table(
    columns: (auto, auto, 1fr), stroke: none, inset: 5pt,
    ..events.sorted(key: e => e.t).enumerate().map(((i, e)) => ([#(i + 1)], [*#e.title* (#e.date)], e.src)).flatten(),
  )
}

// ---------- the slide ----------
#let c-tech = rgb("#2b6cb0")
#let c-pol = rgb("#c53030")
#let c-cult = rgb("#6b46c1")
#let c-born = rgb("#2f855a")

