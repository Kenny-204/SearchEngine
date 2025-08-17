"use client";
import { Canvas, useFrame, useLoader } from "@react-three/fiber";
import { Suspense, useEffect, useRef, useState } from "react";
import { Mesh } from "three";
import { GLTFLoader } from "three/addons/loaders/GLTFLoader.js";
import {
  Html,
  useProgress,
  useGLTF,
  OrbitControls,
  useAnimations,
  Center,
  Stage,
} from "@react-three/drei";

function Loader() {
  const { progress } = useProgress();
  return <Html center>{progress} % loaded</Html>;
}
function Three() {
  return (
    <div className="fixed top-0 left-0 w-screen h-screen z-0 pointer-events-none select-none overflow-hidden">
      <Canvas>
        {/* <ambientLight intensity={0.2} /> */}
        {/* <directionalLight /> */}
        {/* <directionalLight color="red" position={[0, 0, 5]} /> */}
        {/* <AnimatedBox /> */}
        {/* <OrbitControls
        enableZoom
        autoRotate
        maxPolarAngle={Math.PI / 2}
        minPolarAngle={Math.PI / 2}
      /> */}
        <directionalLight position={[10, 10, 5]} intensity={2} />
        <directionalLight position={[-10, -10, -5]} intensity={1} />
        <directionalLight position={[10, -10, -5]} intensity={1} />
        <directionalLight position={[10, 10, -5]} intensity={1} />
        <directionalLight position={[10, -10, 5]} intensity={1} />
        <pointLight intensity={2} />
        {/* <OrbitControls /> */}
        <Suspense fallback={<Loader />}>
          {/* <Center>
            <Scene />
          </Center> */}
          <Stage
            preset="soft"
            intensity={1}
            environment="city"
            shadows={{ type: "contact", opacity: 0.2, blur: 2 }}
          >
            <Scene />
          </Stage>
        </Suspense>
      </Canvas>
    </div>
  );
}

function AnimatedBox() {
  const myMesh = useRef<Mesh>(null);
  const [active, setActive] = useState(false);
  useFrame(({ clock }) => {
    if (!myMesh.current) return;
    const turn = clock.elapsedTime / (Math.PI * 2);
    const r = Math.PI * 2 * (turn - Math.floor(turn));
    let delta = 0.001;
    if (!active) {
      myMesh.current.rotation.x = r;
      myMesh.current.rotation.y = r;
    } else {
      if (Math.abs(myMesh.current.rotation.x) > delta) {
        myMesh.current.rotation.x = myMesh.current.rotation.x / 1.05;
      } else myMesh.current.rotation.x = 0;
      if (Math.abs(myMesh.current.rotation.y) > delta) {
        myMesh.current.rotation.y = myMesh.current.rotation.y / 1.05;
      } else myMesh.current.rotation.y = 0;
    }
  });
  return (
    <mesh
      onPointerEnter={() => setActive(true)}
      onPointerLeave={() => setActive(false)}
      ref={myMesh}
    >
      <boxGeometry args={[2, 2, 2]} />
      <meshStandardMaterial />
    </mesh>
  );
}

function Scene() {
  const { scene, animations } = useGLTF("/model/sphere-organic/scene.gltf");
  const { ref, mixer, names, actions, clips } = useAnimations(
    animations,
    scene
  );
  useEffect(() => {
    console.log(actions);
    let action = actions?.["Take 001"];
    if (action) {
      action.play();
      action.timeScale = 0.5;
    }
  }, []);
  // const gltf = useLoader(GLTFLoader, "/model/sphere-organic/scene.gltf");
  return <primitive object={scene} />;
}

export default Three;
