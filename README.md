# Introduction

Bonjour à tous, je suis Yahn Bervas et aujourd'hui dans cette vidéo, dans le cadre d'un projet de veille technologique à l'IUT de Bordeaux, je vais vous expliquer le principe de l'algorithme du walker, qui est un algorithme de génération procédurale, puis vous montrer comment le mettre en place sur le moteur de jeu Unity.

# Partie 1 : Explications

Alors je vais commencer par vous expliquer le principe de cet algorithme et pour cela je vais utiliser un schéma que vous pouvez voir à l'écran.
Au début, on commence avec une grille d'une taille défini à l'avance par l'utilisateur ou le développeur. Ici dans mon cas, elle aura une taille de 7x7.
Du coup, l'algorithme du walker est un algorithme qui va placer aléatoirement sur une des cases de la grille un walker, que l'on peut traduire par marcheur, et celui-ci a un nombre de pas limité et défini et qui va venir creuser les cases pour construire notre donjon.
Une fois placé, il creuse la case du départ et la note comme case "entrée", puis il va venir choisir une case aléatoire parmi les cases adjacentes à celle où il se trouve à l'instant t tout en les creusant jusqu'à ce qu'il n'ai plus de pas disponible ou qu'il n'ai plus de cases adjacentes non creusées.
Enfin il va choisir une case aléatoire parmi toute celles qui sont le plus loin de la case de départ, ce qui implique qu'il y a aussi l'algorithme de dijkstra qui est utilisé ici, pour la noter comme case "sortie".
Pour mieux comprendre, je vais vous faire un petit exemple à la main.
Ici, on a notre walker, représenté par un carré, ainsi que son nombre de pas en haut. On vient le placer aléatoirement sur une des cases de la grille, puis on le fait se déplacer jusqu'à ce qu'il n'ai plus de pas et qu'il disparaisse. 

# Partie 2 : Revue du code et des prérequis

Présentation de l'arbre de composants -> ce qu'il faut ajouter dans le projet  
Présentation de la tilemap et comment ajouter des tiles  
Présentation des différentes classes du projet  

Maintenant on va passer sur Unity directement et je vais vous expliquer les prérequis pour pouvoir mettre en place l'algorithme du walker dessus.  
Je vais d'abord commencer par vous expliquer les classes créées et leur utilité.  
Alors nous avons la classe DungeonGenerator, qui va permettre de gérer un peu tout le donjon.  
Cette classe va gérer l'initialisation de la grill, donc l'insertion des données dans la structure choisie ici c'est une liste, ainsi que son affichage, et elle va aussi gérer l'initialisation du walker et permettre de faire tourner les algorithmes du walker et de Dijkstra.  
La classe Walker comme son nom l'indique va pemrettre de créer des walker.  
La classe CellDistance va être utile pour l'algorithme de Dijsktra car elle va permettre d'associer une cellule creusée par le walker et sa distance de la case entrée et va donc permettre de déterminer les possibles cases sortie.  
La classe CellType indique le type d'une cellule donc vide, si c'est un mur, entrée ou sortie.
Et enfin la classe Utils a quelques fonctions utiles statiques notamment pour connaître les cellules adjacentes à une cellule, pour créer une liste à partir d'un entier ou encore mélanger une liste.

Enfin passons sur le moteur de jeu, ici on va voir quels objets il faut créer, ainsi que comment utiliser la tilemap et les tiles palettes pour afficher la grille.  
On voit dans notre arbre de composants une mainCamera qui est de base quand on crée un projet unity et qui va permettre d'afficher l'application.  
On a une grid avec une tilemap nommé dungeon, les tilempas sont comme des calques placés sur un grille défini par unity, où l'on peut y placer des tiles.  
On a un objet vide nommé dungeonController qui va contenir notre script principal DungeonGenerator.  
Et enfin, on a un canvas nommé dijkstraCanvas, qui va nous permettre d'afficher les labels contenant la distance, par rapport à la case entrée, de chaque cellule creusée par le walker.  
Pour finir, on va voir comment créer des tiles.  
Pour cela, il faut d'abord ouvrir la fenêtre des tiles palettes en allant sur Windows -> 2D -> tile palette.  
Ca va vous afficher cette fenêtre, mais de manière flottante, vous pouvez l'accrocher comme moi ou la laisser tel quel.
Allons dans le dossier tiles pour créer un objet, comme un carré comme ceci. Puis vous allez glisser cette objet dans la fenêtre des tiles palettes et l'enregistrer avec le nom que vous le vous voulez. Enfin vous pouvez modifier la couleur de la tile en fonction de son utilité.  

# Conclusion

Merci d'avoir regardé cette vidéo et si vous voulez voir plus en détails le projet, vous le pourrez le trouver sur mon git avec l'URL qui est affiché à l'écran.