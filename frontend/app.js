// ─── BookStore 3D Showcase ───
// Onion Architecture-in 4 qatı konsentrik 3D halqalar kimi vizuallaşdırılır.
// Nüvə (Domain) yaşıl sferadır; kitablar orbitdə üzür; bloom effekti parıltı verir.

import * as THREE from "three";
import { EffectComposer } from "three/addons/postprocessing/EffectComposer.js";
import { RenderPass } from "three/addons/postprocessing/RenderPass.js";
import { UnrealBloomPass } from "three/addons/postprocessing/UnrealBloomPass.js";

const REDUCED_MOTION = window.matchMedia("(prefers-reduced-motion: reduce)").matches;

// Seed kataloqu — konsol tətbiqindəki DbInitializer ilə eynidir
const BOOKS = [
  { title: "Mavi mələklər", author: "Çingiz Abdullayev", price: 12.5, stock: 25, color: 0x38bdf8 },
  { title: "İdeal", author: "İsa Hüseynov", price: 14.0, stock: 18, color: 0xa78bfa },
  { title: "1984", author: "George Orwell", price: 15.9, stock: 40, color: 0xfb7185 },
  { title: "Heyvanıstan", author: "George Orwell", price: 11.3, stock: 30, color: 0xfbbf24 },
  { title: "Dune", author: "Frank Herbert", price: 22.75, stock: 15, color: 0x34d399 },
  { title: "Clean Code", author: "Robert C. Martin", price: 45.0, stock: 10, color: 0x22c55e },
  { title: "Clean Architecture", author: "Robert C. Martin", price: 48.5, stock: 8, color: 0x4ade80 }
];

const LAYERS = [
  { name: "Domain", color: 0x22c55e, radius: 0 },
  { name: "Application", color: 0x38bdf8, radius: 3.2 },
  { name: "Infrastructure", color: 0xa78bfa, radius: 4.6 },
  { name: "Presentation", color: 0xfb7185, radius: 6.0 }
];

// ── Səhnə qurulumu ──
const canvas = document.getElementById("scene");
const renderer = new THREE.WebGLRenderer({ canvas, antialias: true, alpha: false });
renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
renderer.setSize(window.innerWidth, window.innerHeight);

const scene = new THREE.Scene();
scene.background = new THREE.Color(0x020617);
scene.fog = new THREE.FogExp2(0x020617, 0.028);

const camera = new THREE.PerspectiveCamera(55, window.innerWidth / window.innerHeight, 0.1, 200);
camera.position.set(0, 3.2, 13);
camera.lookAt(0, 0, 0);

scene.add(new THREE.AmbientLight(0x94a3b8, 0.5));
const keyLight = new THREE.PointLight(0x22c55e, 220, 60);
keyLight.position.set(6, 8, 8);
scene.add(keyLight);
const fillLight = new THREE.PointLight(0x38bdf8, 120, 60);
fillLight.position.set(-8, -4, 6);
scene.add(fillLight);

// ── Nüvə: Domain sferası ──
const coreGroup = new THREE.Group();
const core = new THREE.Mesh(
  new THREE.IcosahedronGeometry(1.4, 2),
  new THREE.MeshStandardMaterial({
    color: 0x052e16,
    emissive: 0x22c55e,
    emissiveIntensity: 0.9,
    metalness: 0.35,
    roughness: 0.25,
    flatShading: true
  })
);
coreGroup.add(core);

const coreWire = new THREE.Mesh(
  new THREE.IcosahedronGeometry(1.62, 1),
  new THREE.MeshBasicMaterial({ color: 0x22c55e, wireframe: true, transparent: true, opacity: 0.28 })
);
coreGroup.add(coreWire);
scene.add(coreGroup);

// ── Qat halqaları ──
const ringMeshes = [];
for (let i = 1; i < LAYERS.length; i++) {
  const layer = LAYERS[i];
  const ring = new THREE.Mesh(
    new THREE.TorusGeometry(layer.radius, 0.055, 16, 160),
    new THREE.MeshStandardMaterial({
      color: layer.color,
      emissive: layer.color,
      emissiveIntensity: 0.55,
      metalness: 0.6,
      roughness: 0.3,
      transparent: true,
      opacity: 0.85
    })
  );
  ring.rotation.x = Math.PI / 2.15;
  ring.rotation.y = 0.12 * i;
  ring.userData.baseSpeed = 0.0016 * (LAYERS.length - i);
  scene.add(ring);
  ringMeshes.push(ring);

  // Halqa üzərində gəzən "data paketi" sferası
  const orb = new THREE.Mesh(
    new THREE.SphereGeometry(0.13, 16, 16),
    new THREE.MeshBasicMaterial({ color: layer.color })
  );
  orb.userData = { radius: layer.radius, speed: (0.35 + 0.18 * i), phase: i * 2.1, ring };
  scene.add(orb);
  ring.userData.orb = orb;
}

// ── Üzən kitablar ──
const bookGroup = new THREE.Group();
BOOKS.forEach((b, i) => {
  const book = new THREE.Group();
  const coverMat = new THREE.MeshStandardMaterial({
    color: b.color, metalness: 0.25, roughness: 0.5,
    emissive: b.color, emissiveIntensity: 0.22
  });
  const pagesMat = new THREE.MeshStandardMaterial({ color: 0xf8fafc, roughness: 0.9 });
  const body = new THREE.Mesh(new THREE.BoxGeometry(0.72, 1.05, 0.16), coverMat);
  const pages = new THREE.Mesh(new THREE.BoxGeometry(0.66, 0.98, 0.12), pagesMat);
  pages.position.x = 0.045;
  book.add(body, pages);

  const angle = (i / BOOKS.length) * Math.PI * 2;
  const radius = 8.2 + (i % 3) * 0.7;
  book.position.set(Math.cos(angle) * radius, (Math.random() - 0.5) * 4.5, Math.sin(angle) * radius);
  book.rotation.set(Math.random() * 0.6, angle + Math.PI / 2, Math.random() * 0.3 - 0.15);
  book.userData = { angle, radius, floatPhase: Math.random() * Math.PI * 2, spin: 0.15 + Math.random() * 0.2 };
  bookGroup.add(book);
});
scene.add(bookGroup);

// ── Ulduz sahəsi ──
const starCount = 1600;
const starPositions = new Float32Array(starCount * 3);
for (let i = 0; i < starCount; i++) {
  const r = 30 + Math.random() * 60;
  const theta = Math.random() * Math.PI * 2;
  const phi = Math.acos(2 * Math.random() - 1);
  starPositions[i * 3] = r * Math.sin(phi) * Math.cos(theta);
  starPositions[i * 3 + 1] = r * Math.sin(phi) * Math.sin(theta);
  starPositions[i * 3 + 2] = r * Math.cos(phi);
}
const starGeo = new THREE.BufferGeometry();
starGeo.setAttribute("position", new THREE.BufferAttribute(starPositions, 3));
const stars = new THREE.Points(starGeo, new THREE.PointsMaterial({
  color: 0x94a3b8, size: 0.09, transparent: true, opacity: 0.75, sizeAttenuation: true
}));
scene.add(stars);

// ── Bloom post-processing ──
const composer = new EffectComposer(renderer);
composer.addPass(new RenderPass(scene, camera));
const bloom = new UnrealBloomPass(
  new THREE.Vector2(window.innerWidth, window.innerHeight), 0.85, 0.7, 0.12
);
composer.addPass(bloom);

// ── Siçan paralaksı ──
const mouse = { x: 0, y: 0 };
window.addEventListener("pointermove", (e) => {
  mouse.x = (e.clientX / window.innerWidth) * 2 - 1;
  mouse.y = (e.clientY / window.innerHeight) * 2 - 1;
});

// ── Qat seçimi (sol panel) ──
let activeLayer = 0;
const layerCards = document.querySelectorAll(".layer-card");
layerCards.forEach((card) => {
  card.addEventListener("click", () => {
    layerCards.forEach((c) => c.classList.remove("active"));
    card.classList.add("active");
    activeLayer = Number(card.dataset.layer);
  });
});

// ── Kitab siyahısı (sağ panel) ──
const bookList = document.getElementById("bookList");
BOOKS.forEach((b) => {
  const li = document.createElement("li");
  li.className = "book-item";
  li.innerHTML = `
    <div>
      <div class="title">${b.title}</div>
      <div class="author">${b.author}</div>
    </div>
    <div>
      <div class="price">${b.price.toFixed(2)} ₼</div>
      <div class="stock">stok: ${b.stock}</div>
    </div>`;
  bookList.appendChild(li);
});

// ── Sayğac animasiyası ──
function animateCounters() {
  document.querySelectorAll("[data-count]").forEach((el) => {
    const target = Number(el.dataset.count);
    if (REDUCED_MOTION) { el.textContent = target; return; }
    const duration = 1400;
    const start = performance.now();
    function tick(now) {
      const p = Math.min((now - start) / duration, 1);
      el.textContent = Math.round(target * (1 - Math.pow(1 - p, 3)));
      if (p < 1) requestAnimationFrame(tick);
    }
    requestAnimationFrame(tick);
  });
}
animateCounters();

// ── Render dövrü ──
const clock = new THREE.Clock();
function animate() {
  requestAnimationFrame(animate);
  const t = clock.getElapsedTime();
  const speedScale = REDUCED_MOTION ? 0.08 : 1;

  coreGroup.rotation.y += 0.004 * speedScale;
  coreWire.rotation.x += 0.002 * speedScale;
  core.scale.setScalar(1 + Math.sin(t * 1.6) * 0.035 * speedScale);

  ringMeshes.forEach((ring, idx) => {
    ring.rotation.z += ring.userData.baseSpeed * speedScale;
    // Seçilmiş qat parlayır
    const isActive = activeLayer === idx + 1;
    ring.material.emissiveIntensity += ((isActive ? 1.6 : 0.55) - ring.material.emissiveIntensity) * 0.08;

    const orb = ring.userData.orb;
    const a = t * orb.userData.speed * speedScale + orb.userData.phase;
    const r = orb.userData.radius;
    orb.position.set(Math.cos(a) * r, Math.sin(a * 1.7) * 0.35, Math.sin(a) * r * Math.cos(Math.PI / 2.15 - Math.PI / 2));
    orb.position.applyAxisAngle(new THREE.Vector3(1, 0, 0), Math.PI / 2.15 - Math.PI / 2);
  });

  // Domain seçiləndə nüvə güclənir
  core.material.emissiveIntensity += ((activeLayer === 0 ? 1.7 : 0.9) - core.material.emissiveIntensity) * 0.08;

  bookGroup.children.forEach((book) => {
    const d = book.userData;
    d.angle += 0.0011 * speedScale;
    book.position.x = Math.cos(d.angle) * d.radius;
    book.position.z = Math.sin(d.angle) * d.radius;
    book.position.y += Math.sin(t * 0.8 + d.floatPhase) * 0.0035 * speedScale;
    book.rotation.y += 0.003 * d.spin * speedScale;
  });

  stars.rotation.y += 0.00016 * speedScale;

  // Kamera paralaksı
  camera.position.x += (mouse.x * 2.4 - camera.position.x) * 0.03;
  camera.position.y += (3.2 - mouse.y * 1.6 - camera.position.y) * 0.03;
  camera.lookAt(0, 0, 0);

  composer.render();
}
animate();

// ── Ölçü dəyişimi ──
window.addEventListener("resize", () => {
  camera.aspect = window.innerWidth / window.innerHeight;
  camera.updateProjectionMatrix();
  renderer.setSize(window.innerWidth, window.innerHeight);
  composer.setSize(window.innerWidth, window.innerHeight);
});
