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
  // Create a simple but attractive 3D scene
  return (
    <>
      {/* Animated rotating sphere */}
      <AnimatedSphere />
      
      {/* Animated floating particles */}
      <AnimatedParticles />
      
      {/* Ambient light for better visibility */}
      <ambientLight intensity={0.3} />
    </>
  );
}

function AnimatedSphere() {
  const meshRef = useRef<Mesh>(null);
  
  useFrame(({ clock }) => {
    if (meshRef.current) {
      meshRef.current.rotation.x = clock.elapsedTime * 0.5;
      meshRef.current.rotation.y = clock.elapsedTime * 0.3;
    }
  });

  return (
    <mesh ref={meshRef} position={[0, 0, 0]}>
      <sphereGeometry args={[2, 32, 32]} />
      <meshStandardMaterial 
        color="#4f46e5" 
        metalness={0.7} 
        roughness={0.2}
        emissive="#1e40af"
        emissiveIntensity={0.2}
      />
    </mesh>
  );
}

function AnimatedParticles() {
  const particlesRef = useRef<Mesh[]>([]);
  
  useFrame(({ clock }) => {
    particlesRef.current.forEach((particle, i) => {
      if (particle) {
        // Make particles float up and down
        particle.position.y = Math.sin(clock.elapsedTime + i * 0.5) * 2;
        // Make particles rotate
        particle.rotation.x = clock.elapsedTime * 0.5 + i * 0.1;
        particle.rotation.y = clock.elapsedTime * 0.3 + i * 0.1;
      }
    });
  });

  return (
    <>
      {Array.from({ length: 30 }, (_, i) => (
        <mesh
          key={i}
          ref={(el) => {
            if (el) particlesRef.current[i] = el;
          }}
          position={[
            (Math.random() - 0.5) * 15,
            (Math.random() - 0.5) * 10,
            (Math.random() - 0.5) * 15
          ]}
        >
          <sphereGeometry args={[0.15, 8, 8]} />
          <meshStandardMaterial 
            color="#8b5cf6" 
            emissive="#8b5cf6"
            emissiveIntensity={0.8}
            transparent={true}
            opacity={0.8}
          />
        </mesh>
      ))}
    </>
  );
}

export default Three;
