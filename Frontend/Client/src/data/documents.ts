import { SearchDocument } from '../types';

// This simulates loading local text files - in a real implementation,
// these would be loaded from actual .txt files
export const documents: SearchDocument[] = [
  {
    id: '1',
    title: 'History of Science',
    filename: 'history_of_science.txt',
    content: `The history of science is the development of science and scientific knowledge over time. Science is a body of empirical, theoretical, and practical knowledge about the natural world, produced by scientists who emphasize the observation, explanation, and prediction of real-world phenomena.

The earliest roots of science can be traced to Ancient Egypt and Mesopotamia in around 3000 to 1200 BCE. Starting in around 3000 BCE, the ancient Egyptians developed a decimal system and advanced mathematics. They also made significant advances in medicine, astronomy, and engineering.

Ancient Greek natural philosophy laid important foundations for modern science. Greek philosophers like Aristotle made systematic observations of the natural world and developed theories to explain natural phenomena. The scientific method as we know it today began to take shape during the Scientific Revolution of the 16th and 17th centuries.

Modern science emerged during the Renaissance and was greatly influenced by the work of scientists like Galileo Galilei, Johannes Kepler, and Isaac Newton. These scientists emphasized empirical observation and mathematical analysis in understanding natural phenomena.`
  },
  {
    id: '2',
    title: 'Introduction to Physics',
    filename: 'intro_to_physics.txt',
    content: `Physics is the natural science that studies matter, its motion and behavior through space and time, and the related entities of energy and force. Physics is one of the most fundamental scientific disciplines, and its main goal is to understand how the universe works.

Classical mechanics, developed primarily by Isaac Newton, describes the motion of objects from projectiles to parts of machinery, as well as astronomical objects like planets, moons, and stars. Classical mechanics provides accurate results when studying large objects that are not extremely massive and speeds that are not approaching the speed of light.

Modern physics began in the early 20th century with Einstein's theory of relativity and the development of quantum mechanics. Einstein's special theory of relativity changed our understanding of space and time, while quantum mechanics revolutionized our understanding of atomic and subatomic particles.

Thermodynamics deals with heat, work, temperature, and energy. The fundamental laws of thermodynamics govern the behavior of these quantities and provide insight into everyday phenomena as well as the workings of engines and refrigerators.`
  },
  {
    id: '3',
    title: 'Computer Programming Fundamentals',
    filename: 'programming_fundamentals.txt',
    content: `Computer programming is the process of creating a set of instructions that tell a computer how to perform a task. Programming involves activities such as analysis, developing algorithms, profiling algorithms' accuracy and resource consumption, and implementing algorithms in a chosen programming language.

The source code of a program is written in one or more programming languages. The purpose of programming is to find a sequence of instructions that will automate the performance of a task on a computer. The process of programming requires knowledge of application domains, algorithms, and programming language expertise.

Programming languages have evolved significantly over the decades. Early programming languages were very close to machine code, requiring programmers to work with binary numbers and memory addresses. Modern high-level programming languages like Python, JavaScript, and Java provide abstractions that make programming more accessible and efficient.

Data structures are fundamental concepts in computer science and programming. Arrays, linked lists, stacks, queues, trees, and graphs are examples of data structures that help organize and store data efficiently. Understanding these concepts is crucial for writing efficient and maintainable code.`
  },
  {
    id: '4',
    title: 'Environmental Science Overview',
    filename: 'environmental_science.txt',
    content: `Environmental science is an interdisciplinary field that combines physical, biological, and information sciences to study the environment and solve environmental problems. This field integrates knowledge from many disciplines including biology, chemistry, physics, ecology, atmospheric science, earth science, and geography.

Climate change represents one of the most significant environmental challenges of our time. The increase in greenhouse gases like carbon dioxide and methane in Earth's atmosphere is causing global temperatures to rise, leading to melting ice caps, rising sea levels, and changing weather patterns.

Biodiversity refers to the variety of life on Earth, encompassing the different species of plants, animals, and microorganisms, the genetic variation within species, and the variety of ecosystems they form. Human activities have accelerated the rate of species extinction, leading to what many scientists call the sixth mass extinction.

Sustainable development seeks to meet the needs of the present without compromising the ability of future generations to meet their own needs. This concept requires balancing economic growth, environmental protection, and social equity. Renewable energy sources like solar, wind, and hydroelectric power play a crucial role in sustainable development.`
  },
  {
    id: '5',
    title: 'Modern Art Movements',
    filename: 'modern_art_movements.txt',
    content: `Modern art encompasses artistic styles and movements that emerged roughly from the 1860s to the 1970s. This period saw dramatic changes in artistic techniques, subjects, and philosophies, often challenging traditional artistic conventions and academic standards.

Impressionism, which began in France in the 1860s, marked a radical departure from traditional painting. Artists like Claude Monet, Pierre-Auguste Renoir, and Edgar Degas focused on capturing the effects of light and color in their paintings, often working outdoors to paint scenes directly from nature.

Cubism, pioneered by Pablo Picasso and Georges Braque in the early 20th century, revolutionized artistic representation by depicting subjects from multiple perspectives simultaneously. This movement broke down objects into geometric forms and reassembled them in abstract arrangements.

Abstract Expressionism emerged in America in the 1940s and 1950s, with artists like Jackson Pollock and Mark Rothko creating large-scale paintings that emphasized emotional expression through color, form, and gesture rather than recognizable subject matter. This movement established New York as a major center of the international art world.`
  },
  {
    id: '6',
    title: 'World Literature Classics',
    filename: 'world_literature.txt',
    content: `World literature encompasses the great works of literature from cultures around the globe, representing diverse perspectives, themes, and artistic traditions. These works have shaped human understanding and continue to influence readers across cultures and generations.

Ancient Greek literature laid the foundation for Western literary tradition. Homer's epic poems, the Iliad and the Odyssey, established many conventions of narrative poetry and heroic literature. Greek tragedies by Sophocles, Euripides, and Aeschylus explored themes of fate, justice, and human nature that remain relevant today.

The novel as a literary form developed significantly during the 18th and 19th centuries. Authors like Jane Austen, Charles Dickens, Leo Tolstoy, and Fyodor Dostoevsky created complex narratives that examined social issues, human psychology, and moral questions through detailed character development and intricate plots.

Contemporary world literature reflects the increasingly globalized nature of modern culture. Authors like Gabriel García Márquez, Chinua Achebe, and Haruki Murakami have brought unique cultural perspectives to international audiences, often blending traditional storytelling techniques with modern narrative innovations.`
  },
  {
    id: '7',
    title: 'Economic Theory Basics',
    filename: 'economic_theory.txt',
    content: `Economics is the social science that studies how societies allocate scarce resources among competing uses. Economic theory provides frameworks for understanding human behavior in markets and analyzing the effects of various policies and institutions on economic outcomes.

Supply and demand form the fundamental basis of market economics. The law of supply states that as prices increase, producers are willing to supply more of a good or service. The law of demand states that as prices increase, consumers demand less. The interaction of supply and demand determines market prices and quantities.

Macroeconomics focuses on the economy as a whole, studying phenomena like inflation, unemployment, economic growth, and monetary policy. Key macroeconomic indicators include Gross Domestic Product (GDP), unemployment rates, and inflation measures. Central banks use monetary policy tools to influence economic conditions.

Microeconomics examines individual economic units such as households and firms. It analyzes how consumers make purchasing decisions, how firms decide on production levels and pricing, and how markets coordinate these individual decisions. Market structures like monopoly, oligopoly, and perfect competition affect economic outcomes.`
  },
  {
    id: '8',
    title: 'Human Psychology Principles',
    filename: 'psychology_principles.txt',
    content: `Psychology is the scientific study of mind and behavior. It encompasses the biological influences, social pressures, and environmental factors that affect how people think, act, and feel. Modern psychology employs scientific methods to understand and explain human behavior.

Cognitive psychology focuses on mental processes such as perception, memory, learning, and problem-solving. Research in this field has revealed how people process information, form memories, and make decisions. Understanding cognitive processes helps explain why people sometimes make irrational choices or fall victim to cognitive biases.

Social psychology examines how people's thoughts, feelings, and behaviors are influenced by others. Topics include conformity, obedience, prejudice, and group dynamics. Social psychologists study how individuals behave differently in groups compared to when alone, and how social norms and expectations shape behavior.

Developmental psychology studies how people change throughout their lives, from infancy to old age. This field examines physical, cognitive, and social development, investigating how genetic factors and environmental influences interact to shape human development. Understanding developmental stages helps inform education, parenting, and healthcare practices.`
  },
  {
    id: '9',
    title: 'Space Exploration Timeline',
    filename: 'space_exploration.txt',
    content: `Space exploration represents humanity's quest to understand the cosmos and our place within it. From ancient astronomical observations to modern space missions, humans have continuously sought to explore beyond our planet and unlock the mysteries of the universe.

The Space Age began on October 4, 1957, when the Soviet Union launched Sputnik 1, the first artificial satellite. This achievement marked the beginning of the space race between the United States and the Soviet Union, spurring rapid advances in rocket technology and space science.

The Apollo program achieved humanity's greatest space exploration milestone when Apollo 11 landed on the Moon on July 20, 1969. Neil Armstrong and Buzz Aldrin became the first humans to walk on another celestial body, while Michael Collins orbited above. This achievement demonstrated that humans could travel to and explore other worlds.

Modern space exploration involves international cooperation through projects like the International Space Station and robotic missions to Mars, Jupiter, and Saturn. Private companies like SpaceX have revolutionized space technology with reusable rockets, making space access more affordable and opening new possibilities for commercial space activities and Mars colonization.`
  },
  {
    id: '10',
    title: 'Renewable Energy Technologies',
    filename: 'renewable_energy.txt',
    content: `Renewable energy technologies harness naturally occurring energy sources that replenish themselves over time. These technologies are crucial for addressing climate change and reducing dependence on fossil fuels while providing sustainable energy solutions for growing global energy demands.

Solar energy technology converts sunlight directly into electricity using photovoltaic cells or concentrates solar thermal energy for heating and power generation. Solar panel efficiency has improved dramatically over the past decades while costs have decreased significantly, making solar power competitive with traditional energy sources in many regions.

Wind energy harnesses the kinetic energy of moving air through wind turbines. Modern wind turbines are highly efficient and can generate electricity even at relatively low wind speeds. Offshore wind farms take advantage of stronger and more consistent winds over oceans, though they require specialized installation and maintenance techniques.

Hydroelectric power generation uses flowing water to turn turbines and generate electricity. Large hydroelectric dams can provide massive amounts of clean energy, but they also have environmental impacts on river ecosystems. Small-scale hydroelectric projects and run-of-river systems offer more environmentally friendly alternatives for local power generation.`
  }
];