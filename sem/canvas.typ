// ================= helpers =================
#let L = x => if type(x) == length { x } else { x * 1pt }       // 5 -> 5pt
#let key(..ids) = ids.pos().map(str).sorted().join("-")        // unordered key

#let palette = (
  C: yellow,
  L: rgb("#d9a066"),
  R: rgb("#6fa8f0"),
  S: rgb("#5fe05f"),
  E: rgb("#f04040"),
  M: rgb("#c58af9"),
)
#let gate-clr = rgb(255, 0, 255)
#let gate-w = 8pt

// ================= setup ops =================
#let def-border(..o) = s => {
  s.border = (:..s.border, ..o.named())
  s
}
#let def-stack(..o) = s => {
  s.stack-def = (pos: (0, 0), n: 4, w: 70, h: 30, title: [Stack], ..o.named())
  s
}
#let def-hist(..o) = s => {
  s.hist-def = (pos: (0, 0), size: 20pt, prefix: [Historie = ], ..o.named())
  s
}

// ================= geometry ops =================
#let V(x, y, ..o) = s => {
  // vertex; number = order of creation
  s.n += 1
  s.v.insert(str(s.n), (
    id: s.n,
    x: L(x),
    y: L(y),
    vis: true,
    clr: black,
    r: 3pt,
    label: auto,
    angle: auto,
    off: (0, 0),
    ..o.named(),
  ))
  s
}
#let T(a, b, c, ..o) = s => {
  s.t.insert(key(a, b, c), (i: (a, b, c), clr: white, stroke: 0.7pt + gray.darken(30%), ..o.named()))
  s
}
#let E(a, b, ..o) = s => {
  s.e.insert(key(a, b), (i: (a, b), clr: green, w: 2.5pt, dash: none, ..o.named()))
  s
}
#let set-v(i, ..o) = s => {
  s.v.at(str(i)) = (:..s.v.at(str(i)), ..o.named())
  s
}
#let set-t(a, b, c, ..o) = s => {
  let k = key(a, b, c)
  s.t.at(k) = (:..s.t.at(k), ..o.named())
  s
}
#let set-e(a, b, ..o) = s => {
  let k = key(a, b)
  s.e.at(k) = (..s.e.at(k), ..o.named())
  s
}
#let del-v(i) = s => {
  let _ = s.v.remove(str(i))
  s
}
#let del-t(a, b, c) = s => {
  let _ = s.t.remove(key(a, b, c))
  s
}
#let del-e(a, b) = s => {
  let _ = s.e.remove(key(a, b))
  s
}

#let show-v(i, ..o) = set-v(i, vis: true, ..o.named())
#let hide-v(i) = set-v(i, vis: false)

// ================= gate / CLERS =================
#let gate(a, b, ..o) = s => {
  s.e.insert("gate", (i: (a, b), clr: gate-clr, w: gate-w, dash: none, ..o.named()))
  s
}
#let no-gate = s => {
  if "gate" in s.e { let _ = s.e.remove("gate") }
  s
}
#let clers(a, b, c, sym, ..o) = set-t(a, b, c, clr: palette.at(sym), label: sym, ..o.named())

// ================= stack =================
#let push(a, b, ..o) = s => {
  // cell "a-b" + dashed highlighted edge
  s.stack.push((a, b))
  s.e.insert(key(a, b), (i: (a, b), clr: gate-clr, w: gate-w, dash: "dotted", ..o.named()))
  s
}
#let pop() = s => {
  assert(s.stack.len() > 0, message: "pop() on empty stack")
  let (a, b) = s.stack.last()
  s.stack = s.stack.slice(0, -1)
  if key(a, b) in s.e { let _ = s.e.remove(key(a, b)) }
  s
}

// ================= history =================
#let add-h(sym, hl: true) = s => {
  s.h.push(sym)
  if hl { s.hh = s.h.len() - 1 }
  s
}
#let remove-h() = s => {
  s.h = s.h.slice(0, -1)
  if s.hh != none and s.hh >= s.h.len() { s.hh = none }
  s
}
#let highlight-h(..a, i) = s => {
  // let i = if a.pos().len() == 0 { -1 } else { a.pos().first() }
  // let i = 0
  s.hh = if i == none { none } else if i < 0 { s.h.len() + i } else { i }
  s
}
#let set-h(str-hist) = s => {
  s.h = str-hist.clusters()
  s.hh = none
  s
}

// ================= script -> states =================
#let next = "next"                          // = #pause

#let anim(..items) = {
  let states = ()
  let s = (
    v: (:),
    t: (:),
    e: (:),
    n: 0,
    h: (),
    hh: none,
    stack: (),
    stack-def: none,
    hist-def: none,
    border: (clr: black, w: 2.5pt),
  )
  let dirty = false
  for it in items.pos().flatten() {
    if it == "next" {
      if dirty {
        states.push(s)
        dirty = false
      }
    } else {
      s = it(s)
      dirty = true
    }
  }
  if dirty { states.push(s) }
  // positions + triangles over ALL states -> stable label placement
  let vpos = (:)
  let all = ()
  for st in states {
    for (k, p) in st.v { vpos.insert(k, (p.x, p.y)) }
    all += st.t.values().map(t => t.i)
  }
  states.map(st => (..st, vpos: vpos, all: all))
}

// ================= drawing =================
#let draw(
  s,
  size: (100%, 60%),
  y-up: true,
  axes: false,
  labels: true,
  label-gap: 5pt,
  label-size: 18pt,
  cell-size: 16pt,
  tri-label-size: 26pt,
  tri-label-clr: gray.darken(80%),
) = {
  let sg = if y-up { -1 } else { 1 }
  block(width: size.at(0), height: size.at(1), layout(sz => {
    let (cx, cy) = (sz.width / 2, sz.height / 2)
    let xy = (x, y) => (cx + x, cy + sg * y)
    let pt = k => {
      let p = s.v.at(str(k))
      xy(p.x, p.y)
    }
    let gp = k => {
      let p = s.vpos.at(str(k))
      xy(p.at(0), p.at(1))
    }
    let at-center = (x, y, body) => context {
      let m = measure(body)
      place(top + left, dx: x - m.width / 2, dy: y - m.height / 2, body)
    }
    // widest free angular gap around vertex k (uses every triangle of the script)
    let best-angle = k => {
      let (x0, y0) = gp(k)
      let angs = ()
      for tr in s.all {
        if k in tr {
          for j in tr.filter(j => j != k) {
            let (x1, y1) = gp(j)
            angs.push(calc.atan2((x1 - x0) / 1pt, (y1 - y0) / 1pt).rad())
          }
        }
      }
      if angs.len() == 0 { return -calc.pi / 4 }
      angs = angs.sorted()
      let n = angs.len()
      let best = (gap: -1.0, ang: 0.0)
      for i in range(n) {
        let a = angs.at(i)
        let gap = if i < n - 1 { angs.at(i + 1) - a } else { angs.at(0) - a + 2 * calc.pi }
        if gap > best.gap { best = (gap: gap, ang: a + gap / 2) }
      }
      best.ang
    }

    if axes {
      place(top + left, line(start: (0pt, cy), end: (sz.width, cy), stroke: 0.3pt + gray))
      place(top + left, line(start: (cx, 0pt), end: (cx, sz.height), stroke: 0.3pt + gray))
    }

    // 1) triangles
    for t in s.t.values() {
      place(top + left, polygon(fill: t.clr, stroke: t.stroke, ..t.i.map(pt)))
    }
    // 2) triangle labels (CLERS letters)
    for t in s.t.values() {
      let l = t.at("label", default: none)
      if l != none {
        let ps = t.i.map(pt)
        let gx = ps.map(p => p.at(0)).sum() / 3
        let gy = ps.map(p => p.at(1)).sum() / 3
        at-center(gx, gy, text(size: tri-label-size, weight: "bold", fill: tri-label-clr, [#l]))
      }
    }
    // 3) border = edges with exactly one (non-hole) triangle
    let cnt = (:)
    for t in s.t.values() {
      if not t.at("hole", default: false) {
        let (a, b, c) = t.i
        for p in ((a, b), (b, c), (c, a)) {
          let k = key(..p)
          cnt.insert(k, (p: p, n: cnt.at(k, default: (n: 0)).n + 1))
        }
      }
    }
    for e in cnt.values().filter(e => e.n == 1) {
      let (p, q) = e.p.map(pt)
      place(top + left, line(start: p, end: q, stroke: stroke(
        paint: s.border.clr,
        thickness: s.border.w,
        cap: "round",
      )))
    }
    // 4) highlighted edges (gate, stack gates, ...)
    for e in s.e.values() {
      let (p, q) = e.i.map(pt)
      let dash = e.at("dash", default: none)
      place(top + left, line(start: p, end: q, stroke: stroke(
        paint: e.clr,
        thickness: e.w,
        cap: if dash == none { "round" } else { "butt" },
        dash: dash,
      )))
    }
    // 5) visible vertices + labels
    for p in s.v.values() {
      if p.vis {
        let (x, y) = pt(p.id)
        place(top + left, dx: x - p.r, dy: y - p.r, circle(radius: p.r, fill: p.clr, stroke: none))
      }
    }
    if labels {
      for p in s.v.values() {
        if p.vis and p.label != none {
          let (x, y) = pt(p.id)
          let th = if p.angle == auto { best-angle(p.id) } else { sg * p.angle.rad() }
          let (ox, oy) = p.off.map(L)
          let d = p.r + label-gap
          at-center(
            x + d * calc.cos(th) + ox,
            y + d * calc.sin(th) + sg * oy,
            text(size: label-size, if p.label == auto { [#p.id] } else { p.label }),
          )
        }
      }
    }
    // 6) stack: n cells, bottom -> top, `pos` = centre of the column
    if s.stack-def != none {
      let d = s.stack-def
      let (w, h) = (L(d.w), L(d.h))
      let (px, py) = d.pos.map(L)
      let n = calc.max(d.n, s.stack.len())
      for j in range(n) {
        let (x, y) = xy(px, py + (j - n / 2 + 0.5) * h)
        let filled = j < s.stack.len()
        let head = j == s.stack.len() - 1
        place(top + left, dx: x - w / 2, dy: y - h / 2, rect(
          width: w,
          height: h,
          inset: 0pt,
          stroke: 1pt + black,
          fill: if head { gate-clr.lighten(60%) } else { none },
          align(center + horizon, if filled {
            text(size: cell-size, str(s.stack.at(j).at(0)) + "-" + str(s.stack.at(j).at(1)))
          } else { [] }),
        ))
      }
      let (tx, ty) = xy(px, py + n / 2 * h + 14pt)
      at-center(tx, ty, text(size: cell-size, d.title))
    }
    // 7) history string; `pos` = top-left corner
    if s.hist-def != none {
      let d = s.hist-def
      let (hx, hy) = d.pos.map(L)
      let (x, y) = xy(hx, hy)
      let syms = s
        .h
        .enumerate()
        .map(((i, c)) => if i == s.hh {
          box(fill: palette.at(c, default: yellow), inset: (x: 2pt), outset: (y: 3pt), radius: 2pt, c)
        } else { c })
      place(top + left, dx: x, dy: y, text(size: d.size, {
        d.prefix
        syms.join()
      }))
    }
  }))
}

// ================= caption chips =================
#let eb-tag(clr, body, fg: black) = box(
  fill: clr,
  inset: (x: 3pt),
  outset: (y: 3pt),
  radius: 2pt,
  text(fill: fg, weight: "bold", body),
)
#let eb-sym(s) = eb-tag(palette.at(upper(s)), upper(s))
#let gate-tag() = eb-tag(gate-clr, [_gate_])
