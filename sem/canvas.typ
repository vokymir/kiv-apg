// ---------- constructors with defaults ----------
#let v(x, y, clr: black, radius: 1.5pt, label: auto, label-angle: auto, label-offset: (0pt, 0pt)) = (
  x: x,
  y: y,
  clr: clr,
  radius: radius,
  label: label,
  label-angle: label-angle,
  label-offset: label-offset,
)

#let tri(a, b, c, clr: white, stroke: black) = (i: (a, b, c), clr: clr, stroke: stroke)

// edges are *highlights* drawn over the triangle outlines
#let edge(a, b, clr: green, thickness: 2.5pt) = (i: (a, b), clr: clr, thickness: thickness)

#let make-mesh(vertices: (), triangles: (), edges: ()) = (vertices: vertices, triangles: triangles, edges: edges)

// ---------- normalisation (accepts arrays and partial dicts) ----------
#let as-v(p) = if type(p) == array {
  v(p.at(0), p.at(1))
} else {
  (clr: black, radius: 1.5pt, ..p)
}

// ---------- drawing ----------
#let draw(
  mesh,
  size: (100%, 60%),
  y-up: true,
  axes: false,
  labels: true, // vertex indices
  label-gap: 5pt,
  label-size: 18pt,
  label-base: 0,
  tri-labels: true, // triangle indices at the centroid
  tri-label-size: 28pt,
  tri-label-clr: gray.darken(80%),
) = {
  let vs = mesh.vertices.map(as-v)
  let sy = if y-up { -1 } else { 1 }

  block(width: size.at(0), height: size.at(1), stroke: 0.5pt + gray, layout(sz => {
    let (cx, cy) = (sz.width / 2, sz.height / 2)
    let pt = k => (cx + vs.at(k).x, cy + sy * vs.at(k).y)

    // body centred on canvas point (x, y)
    let at-center = (x, y, body) => context {
      let m = measure(body)
      place(top + left, dx: x - m.width / 2, dy: y - m.height / 2, body)
    }

    // widest free angular gap around vertex k (canvas coordinates, radians)
    let best-angle = k => {
      let (x0, y0) = pt(k)
      let angs = ()
      for tr in mesh.triangles {
        if k in tr.i {
          for j in tr.i.filter(j => j != k) {
            let (x1, y1) = pt(j)
            angs.push(calc.atan2((x1 - x0) / 1pt, (y1 - y0) / 1pt).rad())
          }
        }
      }
      if angs.len() == 0 {
        -calc.pi / 4
      } else {
        let angs = angs.sorted()
        let n = angs.len()
        let best = (gap: -1.0, ang: 0.0)
        for i in range(n) {
          let a = angs.at(i)
          let gap = if i < n - 1 { angs.at(i + 1) - a } else { angs.at(0) - a + 2 * calc.pi }
          if gap > best.gap { best = (gap: gap, ang: a + gap / 2) }
        }
        best.ang
      }
    }

    if axes {
      place(top + left, line(start: (0pt, cy), end: (sz.width, cy), stroke: 0.3pt + gray))
      place(top + left, line(start: (cx, 0pt), end: (cx, sz.height), stroke: 0.3pt + gray))
    }

    // 1) triangles
    for t in mesh.triangles {
      let t = (clr: white, stroke: black, ..t)
      place(top + left, polygon(fill: t.clr, stroke: t.stroke, ..t.i.map(pt)))
    }

    // 2) triangle numbers (centroid)
    if tri-labels {
      for (n, t) in mesh.triangles.enumerate() {
        let l = t.at("label", default: auto)
        if l != none {
          let ps = t.i.map(pt)
          let gx = ps.map(p => p.at(0)).sum() / 3
          let gy = ps.map(p => p.at(1)).sum() / 3
          at-center(gx, gy, text(size: tri-label-size, fill: tri-label-clr, if l == auto { [#n] } else { l }))
        }
      }
    }

    // 3) highlighted edges
    for e in mesh.at("edges", default: ()) {
      let e = (clr: green, thickness: 2.5pt, ..e)
      let (a, b) = e.i.map(pt)
      place(top + left, line(
        start: a,
        end: b,
        stroke: stroke(paint: e.clr, thickness: e.thickness, cap: "round"),
      ))
    }

    // 4) vertices
    for (k, p) in vs.enumerate() {
      let (x, y) = pt(k)
      place(top + left, dx: x - p.radius, dy: y - p.radius, circle(radius: p.radius, fill: p.clr, stroke: none))
    }

    // 5) vertex labels
    if labels {
      for (k, p) in vs.enumerate() {
        let l = p.at("label", default: auto)
        if l != none {
          let (x, y) = pt(k)
          let ang = p.at("label-angle", default: auto)
          let th = if ang == auto { best-angle(k) } else { sy * ang.rad() }
          let (ox, oy) = p.at("label-offset", default: (0pt, 0pt))
          let d = p.radius + label-gap
          at-center(
            x + d * calc.cos(th) + ox,
            y + d * calc.sin(th) + sy * oy,
            text(size: label-size, if l == auto { [#(k + label-base)] } else { l }),
          )
        }
      }
    }
  }))
}
// ===================== EDGEBREAKER VOE ========================

#let eb-style = (
  clers: (
    C: yellow,
    L: rgb("#d9a066"),
    R: rgb("#6fa8f0"),
    S: rgb("#5fe05f"),
    E: rgb("#f04040"),
  ),
  gate: green.darken(10%),
  gate-thickness: 3pt, // current gate
  prev-gate: rgb("#8b5a2b"),
  prev-thickness: 3pt, // earlier gates
  vertex: green.darken(10%),
  vertex-radius: 3.5pt, // highlighted opposite vertex
)

#let eb-step(m, gate, sym, st: eb-style, tri-idx: auto) = {
  // 1) which triangle?
  let t = if tri-idx != auto { tri-idx } else {
    let c = m.triangles.enumerate().filter(((k, tr)) => gate.all(g => g in tr.i) and not tr.at("done", default: false))
    assert(c.len() > 0, message: "no unprocessed triangle on gate " + repr(gate))
    c.first().at(0)
  }
  // 2) opposite vertex = the one not on the gate
  let opp = m.triangles.at(t).i.find(k => k not in gate)
  let old-vertex = m.vertices.at(opp)

  for v in range(m.vertices.len()) {
    let o = m.vertices.at(v)
    m.vertices.at(v) = (..as-v(o), clr: black, radius: st.vertex-radius)
  }

  // state A: gate + vertex highlight, previous gates -> brown
  let a = m
  a.edges = a.edges.map(e => if e.at("role", default: none) == "gate" {
    (..e, clr: st.prev-gate, thickness: st.prev-thickness)
  } else { e })
  a.edges.push((
    ..edge(gate.at(0), gate.at(1), clr: st.gate, thickness: st.gate-thickness),
    role: "gate",
  ))
  a.vertices.at(opp) = (..as-v(old-vertex), clr: st.vertex, radius: st.vertex-radius)

  // state B: triangle gets its CLERS colour, vertex returns to normal
  let b = a
  b.triangles.at(t) = (..b.triangles.at(t), clr: st.clers.at(upper(sym)), done: true)
  // b.vertices.at(opp) = old-vertex

  (a, b)
}

// run a whole history: ((gate, symbol), ...) -> all states, incl. the initial one
#let eb-run(m, steps, st: eb-style) = {
  let out = (m,)
  for (gate, sym) in steps {
    out += eb-step(out.last(), gate, sym, st: st)
  }
  out
}

#let eb-clear(m) = {
  let m = m
  m.edges = m.edges.filter(e => e.at("role", default: none) != "gate")
  m
}
