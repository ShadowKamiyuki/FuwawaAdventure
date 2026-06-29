# Nombre del juego: Fuwawa Adventure
- Genero: 3D platformer game
- Objetivo: avanzar por el nivel y llegar a la meta
- Sistemas de IA implementados: steearing behaviours, line of sight, FSM basada en clases en 3 enemigos distintos
- Controles: movimiento del personaje con WASD, salto con espacio y movimiento de camara con el mouse

En esta segunda entrega se implemento pathfinding con A* junto a los steearing behaviours para crear 3 enemigos mas "inteligentes" que tengan
comportamientos mas complejos.
Todos los enemigos usan una FSM con interfaz IState para definir los estados concretos. En el caso de la deteccion del jugador todos implementan
line of sight y para el movimiento usan steering behaviours junto a A* pathfinding con nodos para navegar a traves del mapa.

El skeleton tiene 2 estados (patrol y chase) donde patrol es un movimiento entre los nodos del mapa para que siga un camino definido.
Pero en el estado de chase usa pursue directo si tiene el jugador a la vista o A* si pierde la vision. Al volver al estado de patrol, el sistema
de este enemigo hgace que pueda volver a patrullar desde el nodo mas cercano para evitar navegacion descontrolada. Y otra caracteristica es que
al perseguir al jugador este enemigo se detiene a una distancia pequeña del jugador para evitar empujarlo y estar en rango de ataque.

El angry skeleton tiene 4 estados (patrol, chase, attack y flee). Este enemigo tiene un comportamiento un poco mas complejo que el skeleton
basico ya que implementa sistemas de este pero tiene logica de decision para mantenerse siempre a una distancia del jugador y si este se acerca
el enemigo se aleja. Para el chase usa A* y el sistema de rango de combate por lo que integra steering behaviours como flee o arrive en su logica.

Y por ultimo el golden skeleton que seria el mas complejo de todos implementa logica de memoria porque si ve al jugador usa line of sight con
pursue. Pero si pierde de vista al jugador este guarda la ultima posicion donde lo vio para buscarlo utilizando arrive y A*, al terminar el sistema
calcula el nodo mas cercano para volver a la ruta de patrulla. Por esto el enemigo posee 3 estados entre patrol, investigate y chase