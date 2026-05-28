# REQUERIMIENTOS CONSOLIDADOS DEL PROYECTO MONOLISA 2
**Formato Jerárquico - Módulos Funcionales**

---

## MÓDULO 1: INICIO Y TUTORIAL

### Requerimientos Funcionales

**RQF1.1** — El jugador debe iniciar en un menú interactivo que funciona como un tutorial del juego.

**RQF1.2** — El sistema debe generar al personaje en la plataforma de entrada al comenzar cada nivel.

**RQF1.3** — El sistema debe mostrar en pantalla todos los controles disponibles para el jugador (moverse, mirar, saltar, usar poder, cambiar poder, explicación de poder, reiniciar intento, pausa).

**RQF1.4** — El sistema debe definir la salida como el objetivo final del nivel.

**RQF1.5** — El sistema debe mantener la salida inactiva hasta que el jugador recolecta todos los coleccionables del nivel.

**RQF1.6** — El sistema debe activar la salida una vez que todos los coleccionables requeridos sean recolectados.

**RQF1.7** — El jugador debe poder reiniciar el intento actual presionando la tecla `R`.

**RQF1.8** — El jugador debe poder pausar el juego presionando la tecla `ESC`.

**RQF1.9** — El sistema debe cargar automáticamente el fondo del nivel desde `Resources/Canvas_Fondo` si no existe en escena.

**RQF1.10** — El sistema debe asegurar que el fondo cargado tenga sorting order `-100` para aparecer detrás de todos los elementos.

### Requerimientos No Funcionales

**RQNF1.1** — El menú interactivo debe tener un tamaño fijo de 16×29 celdas con cámara fija.

**RQNF1.2** — El sistema debe proveer retroalimentación visual en la salida con puntos grisáceos que cambian a verde para indicar el progreso de recolección.

**RQNF1.3** — Los controles mostrados en el tutorial deben ser legibles y estar posicionados en la zona superior derecha de la pantalla.

**RQNF1.4** — El sistema debe asegurar que la cámara sea ortográfica y esté configurada para la resolución de 16×29 celdas (suponiendo celdas cuadradas).

**RQNF1.5** — El fondo cargado automáticamente debe usar la cámara actual de la escena (Main Camera).

**RQNF1.6** — El sistema debe registrar errores cuando falte el componente Canvas o la cámara no pueda ser encontrada.

---

## MÓDULO 2: CONTROLES DEL JUGADOR

### Requerimientos Funcionales

**RQF2.1** — El jugador debe desplazarse a la izquierda y derecha usando las teclas `←` y `→`.

**RQF2.2** — El jugador debe mirar arriba y abajo con las teclas `↑` y `↓`.

**RQF2.3** — El jugador debe saltar con la tecla `Z`, siempre que esté en el suelo o dentro del margen de "coyote time".

**RQF2.4** — El jugador debe usar el poder seleccionado con la tecla `X`.

**RQF2.5** — El jugador debe cambiar el poder seleccionado manteniendo la tecla `C` y usando `←` o `→`.

**RQF2.6** — El jugador debe mantener presionada la tecla `C` para visualizar la descripción del poder seleccionado.

**RQF2.7** — El sistema debe responder a entradas solo cuando `Time.timeScale != 0` (juego no pausado).

**RQF2.8** — El salto debe usar un "coyote time" de 0.15 segundos para permitir saltos después de abandonar una plataforma.

**RQF2.9** — El movimiento horizontal debe tener una velocidad configurable y consistente.

**RQF2.10** — El sistema debe calcular la altura máxima de salto como 2 bloques de altura (1×1 celdas).

### Requerimientos No Funcionales

**RQNF2.1** — Las entradas de teclado deben procesarse en `Update()` para movimiento y en eventos de tecla para acciones instantáneas.

**RQNF2.2** — El sistema debe normalizar los vectores de movimiento para evitar favorecer direcciones diagonales.

**RQNF2.3** — La velocidad de movimiento debe ser expresada en unidades por segundo y ser modificable.

**RQNF2.4** — El salto debe usar `Rigidbody2D.linearVelocity` para aplicar la velocidad en lugar de `AddForce()`.

**RQNF2.5** — El coyote time debe ser decrementado en cada frame y debe ejecutarse solo en `Update()`.

**RQNF2.6** — Las rotaciones de escala del personaje deben ser suaves y no provocar cambios abruptos de dirección.

---

## MÓDULO 3: PLATAFORMAS Y ELEMENTOS DEL NIVEL

### Requerimientos Funcionales - Plataformas Estáticas

**RQF3.1** — Las plataformas "Estático" no deben poder ser movidas ni alteradas por ningún poder.

**RQF3.2** — Las plataformas "Estático" deben incluir puntos de entrada, salida y otras estructuras clave del nivel.

### Requerimientos Funcionales - Plataformas de Enganche

**RQF3.3** — Las plataformas de "Enganche" deben ser inmóviles por defecto.

**RQF3.4** — El sistema debe permitir que plataformas de "Enganche" sean movidas solo si un poder específico así lo define.

**RQF3.5** — Las plataformas de "Enganche" deben estar completamente rellenas en el interior en diseño.

### Requerimientos Funcionales - Plataformas de Desenganche

**RQF3.6** — El sistema debe aplicar gravedad a las plataformas de "Desenganche".

**RQF3.7** — El jugador debe poder mover plataformas de "Desenganche" mediante contacto o poderes.

**RQF3.8** — El sistema debe hacer desaparecer plataformas de "Desenganche" si caen fuera de los límites del nivel.

**RQF3.9** — Las plataformas de "Desenganche" deben tener un interior hueco y exterior pintado en diseño.

### Requerimientos Funcionales - Plataformas Frágiles

**RQF3.10** — El sistema debe iniciar un proceso de quebrantamiento en 3 fases cuando el jugador o un poder interactúa con una plataforma "Frágil".

**RQF3.11** — Cada fase de quebrantamiento debe durar 2 segundos.

**RQF3.12** — El sistema debe mostrar visualmente el progreso a través de las 3 fases de quebrantamiento.

**RQF3.13** — El sistema debe aplicar vibraciones visuales a la plataforma "Frágil" durante el quebrantamiento final.

**RQF3.14** — Al terminar la tercera fase, el sistema debe convertir la plataforma "Frágil" en "Desenganche" con gravedad.

**RQF3.15** — Las plataformas "Frágiles" deben tener un relleno interior quebrantado en diseño.

### Requerimientos Funcionales - Plataformas Flotantes

**RQF3.16** — Las plataformas "Flotantes" deben ser movidas por el jugador o poderes, respondiendo a un vector de empuje.

**RQF3.17** — El sistema debe hacer que plataformas "Flotantes" vuelvan a un estado estático una vez que termine su recorrido.

**RQF3.18** — Las plataformas "Flotantes" deben recibir impulsos sin aplicarse gravedad durante su movimiento.

**RQF3.19** — Las plataformas "Flotantes" deben tener un tono más oscuro que el color exterior en diseño.

### Requerimientos Funcionales - Coleccionables

**RQF3.20** — El sistema debe registrar un coleccionable como "tomado" cuando el jugador lo toca físicamente.

**RQF3.21** — El sistema debe registrar un coleccionable como "tomado" si el jugador logra sacarlo de los límites jugables de la cámara.

**RQF3.22** — Los coleccionables no recolectados deben mostrar como "grisáceos" en la salida.

**RQF3.23** — Los coleccionables recolectados deben cambiar a color "verde" en la salida.

**RQF3.24** — Los coleccionables deben ser entidades de tamaño 1×1 celdas.

**RQF3.25** — Los coleccionables deben tener estado "flotante" por defecto y este estado no debe poder ser cambiado.

### Requerimientos No Funcionales

**RQNF3.1** — Las plataformas deben ser GameObjects con componentes `Collider2D` (tipo `BoxCollider2D` o similar).

**RQNF3.2** — Las plataformas dinámicas deben tener `Rigidbody2D` con `bodyType` apropiado (`Static`, `Dynamic`, `Kinematic`).

**RQNF3.3** — Las transiciones de estado de plataformas deben hacerse sin jitter ni comportamientos inestables.

**RQNF3.4** — La gravedad debe ser constante en todos los elementos dinámicos: 9.81 m/s² o equivalente configurado.

**RQNF3.5** — El sistema debe evitar múltiples cambios de tipo de rigidbody en el mismo frame.

**RQNF3.6** — Las colisiones deben usar capas (`Layer`) para separar plataformas, coleccionables y el jugador.

**RQNF3.7** — Los coleccionables deben tener un script que detecte salida del área de juego mediante `OnTriggerExit2D`.

**RQNF3.8** — El tiempo de quebrantamiento (2 segundos por fase) debe ser preciso y no afectado por el frame rate.

**RQNF3.9** — La vibraciones de plataformas frágiles deben ser sutiles (intensidad ≤ 0.1 unidades) para no afectar la experiencia visual.

---

## MÓDULO 4: SISTEMA DE ETIQUETAS

### Requerimientos Funcionales

**RQF4.1** — El sistema debe emplear un sistema de etiquetado para clasificar tanto los "poderes" como los "niveles".

**RQF4.2** — El sistema debe usar etiquetas en los poderes para describir el tipo de problema que resuelven.

**RQF4.3** — El sistema debe usar etiquetas en los niveles para señalar los desafíos principales que plantean.

**RQF4.4** — El sistema debe implementar la etiqueta "Horizontal" para poderes relacionados con el desplazamiento lateral.

**RQF4.5** — El sistema debe implementar la etiqueta "Vertical" para poderes relacionados con el desplazamiento vertical o alcance de altura.

**RQF4.6** — El sistema debe implementar la etiqueta "general" para definir niveles que combinan desafíos horizontales y verticales.

**RQF4.7** — El sistema debe implementar la etiqueta "new_gravedad" para poderes o niveles que alteran la gravedad.

**RQF4.8** — El sistema debe implementar la etiqueta "impulsar" para definir la acción de empujar al jugador o un objeto.

**RQF4.9** — El sistema debe implementar la etiqueta "crear_plataforma" para poderes que generan nuevas plataformas.

**RQF4.10** — El sistema debe implementar la etiqueta "cruzar_abismo" para desafíos que requieren cruzar espacios largos.

**RQF4.11** — El sistema debe implementar la etiqueta "teletransportacion" para poderes que cambian la posición del jugador a una guardada.

**RQF4.12** — El sistema debe implementar la etiqueta "único" para poderes que solo pueden usarse una vez.

**RQF4.13** — El sistema debe implementar la etiqueta "medidor" para poderes cuya carga disminuye mientras se mantiene presionado el botón de uso.

**RQF4.14** — El sistema debe implementar la etiqueta "varios{3-5}" para poderes con un número definido de usos múltiples.

**RQF4.15** — El sistema debe mantener un diccionario de relaciones entre etiquetas de nivel y etiquetas de poderes en `LogicaPoderes.Relaciones`.

### Requerimientos No Funcionales

**RQNF4.1** — Las etiquetas deben ser strings case-insensitive para evitar inconsistencias.

**RQNF4.2** — El sistema debe usar enums o constantes para etiquetas en lugar de strings mágicos en el código.

**RQNF4.3** — El diccionario de relaciones debe ser serializable y accesible desde el inspector de Unity.

**RQNF4.4** — Las etiquetas deben ser documentadas en un enum `TipoEtiqueta` para mayor mantenibilidad.

---

## MÓDULO 5: INTERFAZ E INDICADORES VISUALES

### Requerimientos Funcionales

**RQF5.1** — El sistema debe mostrar el poder actualmente seleccionado en una interfaz en la esquina inferior derecha.

**RQF5.2** — El jugador debe poder mantener presionada la tecla `C` para expandir la interfaz de información del poder.

**RQF5.3** — El sistema debe expandir la interfaz si se presiona por más de 2 segundos.

**RQF5.4** — El sistema debe mostrar la descripción del poder seleccionado cuando la interfaz está expandida.

**RQF5.5** — El jugador debe poder usar las teclas de movimiento izquierda y derecha para navegar y ver las descripciones de otros poderes mientras la interfaz está expandida.

**RQF5.6** — El sistema debe mostrar el progreso de los 9 niveles en la parte superior central.

**RQF5.7** — El sistema debe mostrar indicadores sobre el personaje que reflejen los usos restantes del poder seleccionado.

**RQF5.8** — El sistema debe mostrar círculos amarillos que desaparecen con cada uso para poderes de "varios".

**RQF5.9** — El sistema debe mostrar un medidor horizontal que se consume durante el uso para poderes de "medidor".

**RQF5.10** — El sistema debe reemplazar el indicador de uso por una "X" gris cuando el poder seleccionado ya no tenga usos o carga.

**RQF5.11** — El sistema debe ocultar el panel de poderes en el tutorial.

**RQF5.12** — El sistema debe actualizar el icono y cargas del poder actual en el HUD cada vez que el poder cambia o se usa.

### Requerimientos No Funcionales

**RQNF5.1** — La ruleta de poder debe cubrir un área de 4×5 celdas.

**RQNF5.2** — La ruleta expandida debe cubrir un área de 8×10 celdas.

**RQNF5.3** — Los indicadores de nivel deben usar el color GRIS para los niveles faltantes.

**RQNF5.4** — Los indicadores de nivel deben usar el color AZUL para el nivel actual.

**RQNF5.5** — Los indicadores de nivel deben usar el color VERDE para niveles superados.

**RQNF5.6** — El sistema no debe informar explícitamente al jugador sobre el significado de los colores de los indicadores de nivel.

**RQNF5.7** — Los indicadores de uso deben ordenarse en filas de 3.

**RQNF5.8** — La expansión de la interfaz debe ser suave (transición de escala durante 2 segundos).

**RQNF5.9** — El cambio de poder durante la navegación debe ser instantáneo sin desplazamiento de menú.

**RQNF5.10** — El sistema debe usar animadores o Canvas groups para las transiciones de UI.

---

## MÓDULO 6: ALGORITMO DE EVALUACIÓN DE DESEMPEÑO (SKILL)

### Requerimientos Funcionales

**RQF6.1** — El sistema debe implementar un algoritmo para evaluar el desempeño del jugador en cada nivel.

**RQF6.2** — El sistema debe modificar el layout de niveles futuros basándose en el "skill score" del jugador.

**RQF6.3** — El sistema debe gestionar un "Skill score" numérico de 0 a 100 para el jugador.

**RQF6.4** — El sistema debe iniciar con un "Skill score" de 50 al comenzar una partida.

**RQF6.5** — El sistema debe calcular una `PuntuacionNivel` basada en tres factores: Cantidad de intentos, Uso de poderes y Cantidad de tiempo.

**RQF6.6** — El sistema debe asignar +10 puntos por terminar en el primer intento.

**RQF6.7** — El sistema debe asignar +5 puntos por terminar en el segundo y tercer intento.

**RQF6.8** — El sistema debe asignar +0 puntos por terminar en el cuarto y quinto intento.

**RQF6.9** — El sistema debe asignar -5 puntos por terminar en el sexto, séptimo y octavo intento.

**RQF6.10** — El sistema debe asignar -10 puntos por terminar en el noveno intento o superior.

**RQF6.11** — El sistema debe asignar +10 puntos por terminar un nivel sin usar poderes.

**RQF6.12** — El sistema debe asignar +0 puntos si se usaron de uno a dos poderes.

**RQF6.13** — El sistema debe asignar -5 puntos si se tuvo dependencia de un solo poder (usado más veces).

**RQF6.14** — El sistema debe asignar +5 puntos si se acabó el nivel en menos de 1 minuto.

**RQF6.15** — El sistema debe asignar +0 puntos si se acabó el nivel de 1 a 2 minutos.

**RQF6.16** — El sistema debe asignar -5 puntos si se acabó el nivel en más de 2 minutos.

**RQF6.17** — El sistema debe calcular la `NuevaSkill` del jugador usando la fórmula: `NuevaSkill = (SkillActual * 0.7) + (PuntuacionNivel * 0.3)`.

**RQF6.18** — El sistema debe activar el estado "issue" si la `NuevaSkill` es menor a 35.

**RQF6.19** — El sistema debe activar el estado "based" si la `NuevaSkill` está entre 35 y 65.

**RQF6.20** — El sistema debe activar el estado "solution" si la `NuevaSkill` es mayor a 65.

**RQF6.21** — En estado "issue", el sistema debe reducir la cantidad de plataformas frágiles en niveles futuros.

**RQF6.22** — En estado "issue", el sistema debe aumentar la probabilidad de aparición (en re-roll) de los poderes con los que el jugador más haya ganado.

**RQF6.23** — En estado "solution", el sistema debe aumentar la cantidad de plataformas frágiles en niveles futuros.

**RQF6.24** — En estado "solution", el sistema debe reducir la probabilidad de aparición (en re-roll) de los poderes más ganadores y aumentar la aparición de "condiciones".

**RQF6.25** — El sistema debe mantener un historial de poderes usados durante la partida para influir en probabilidades y puntajes.

### Requerimientos No Funcionales

**RQNF6.1** — La puntuación de intentos debe ser entero y estar entre -10 y +10.

**RQNF6.2** — La puntuación de tiempo debe ser entero y estar entre -5 y +5.

**RQNF6.3** — La puntuación de poderes debe ser entero y estar entre -5 y +10.

**RQNF6.4** — El skill score debe estar acotado entre 0 y 100 sin permitir valores negativos o mayores a 100.

**RQNF6.5** — El cálculo de NuevaSkill debe redondearse a un decimal para evitar precisión excesiva.

**RQNF6.6** — El sistema debe guardar el historial de skill scores por nivel para análisis.

**RQNF6.7** — El cambio de estado ("issue", "based", "solution") debe ser instantáneo al terminar un nivel.

---

## MÓDULO 7: REGISTRO Y EJECUCIÓN DE EVENTOS

### Requerimientos Funcionales

**RQF7.1** — El sistema debe crear un registro temporal al iniciar un nivel.

**RQF7.2** — El registro temporal debe incluir un contador de "Intentos".

**RQF7.3** — El registro temporal debe incluir un contador de "Tiempo".

**RQF7.4** — El registro temporal debe incluir un contador de "Uso de poderes".

**RQF7.5** — El sistema debe calcular la `PuntuacionNivel` al completar el nivel.

**RQF7.6** — El sistema debe calcular la `NuevaSkill` usando su fórmula correspondiente al completar el nivel.

**RQF7.7** — El sistema debe actualizar el estado global del jugador (Issue, Based o Solution).

**RQF7.8** — El estado global actualizado debe comunicarse al sistema de modificación de layout.

**RQF7.9** — El estado global actualizado debe comunicarse al sistema de ajuste de aparición de poderes y condiciones.

**RQF7.10** — El sistema debe reiniciar los contadores temporales (intentos, tiempo, poderes) al pasar al siguiente nivel.

**RQF7.11** — El sistema debe incrementar el contador de intentos cada vez que el jugador inicia un intento desde la pantalla de re-roll.

**RQF7.12** — El sistema debe registrar el tiempo total de partida incluyendo el tiempo en pantalla de re-roll y lock.

**RQF7.13** — El sistema debe registrar un uso de poder cada vez que el jugador ejecuta un poder durante el nivel.

### Requerimientos No Funcionales

**RQNF7.1** — El contador de intentos debe ser de tipo entero no negativo.

**RQNF7.2** — El contador de tiempo debe usar `Time.deltaTime` acumulado, no `Time.timeSinceLevelLoad`.

**RQNF7.3** — El contador de poderes debe ser de tipo entero no negativo.

**RQNF7.4** — El registro temporal debe ser una estructura o clase instanciada por nivel.

**RQNF7.5** — Los contadores deben ser accesibles solo lectura desde componentes externos.

---

## MÓDULO 8: MENÚ PRINCIPAL Y MODOS DE JUEGO

### Requerimientos Funcionales

**RQF8.1** — El sistema debe presentar el menú principal como un nivel jugable con plataformas y 4 puertas.

**RQF8.2** — El jugador debe iniciar en la puerta central inferior, la cual debe quedar inhabilitada después de salir.

**RQF8.3** — El jugador debe recolectar dos coleccionables y alcanzar la puerta "empezar" para iniciar el juego normal.

**RQF8.4** — El jugador debe poder acceder a la puerta "modos de juego" sin necesidad de coleccionables.

**RQF8.5** — El jugador debe poder acceder a la puerta "salir del juego" sin necesidad de coleccionables.

**RQF8.6** — El sistema debe proveer un modo de juego "random crudo" (endless) que elimina los niveles, re-roll, lock y reintentos.

**RQF8.7** — En modo "random crudo", el sistema debe asignar 3 poderes al jugador y registrar la cantidad de niveles consecutivos superados.

**RQF8.8** — El sistema debe proveer un "modo difícil" que mantiene la jugabilidad normal pero fija el algoritmo Skill en modo "solución".

**RQF8.9** — El "modo difícil" debe permanecer bloqueado si el jugador no ha ganado más de 5 veces en el modo normal.

**RQF8.10** — El sistema debe mostrar el mensaje "no has pasado nuestras cámaras lo suficiente" cuando intente acceder a modo difícil sin cumplir requisitos.

**RQF8.11** — El jugador debe poder salir del juego desde la puerta de salida en el menú.

### Requerimientos No Funcionales

**RQNF8.1** — El menú debe tener 11 plataformas, 10 siendo de "enganche" y 1 siendo "frágil".

**RQNF8.2** — El menú debe contener exactamente 4 puertas con funciones definidas.

**RQNF8.3** — El salto base del jugador en el menú debe limitarse a 2 bloques de altura.

**RQNF8.4** — Los modos deben ser accesibles desde la escena "Pantalla_Seleccion".

**RQNF8.5** — El sistema debe guardar el contador de victorias globales para validar acceso a modo difícil.

---

## MÓDULO 9: SELECCIÓN INICIAL DE PODERES Y NIVELES

### Requerimientos Funcionales

**RQF9.1** — El sistema debe seleccionar aleatoriamente 9 poderes de los 15 totales al inicio de una partida normal.

**RQF9.2** — El sistema debe asegurar que los 6 poderes no seleccionados no aparezcan durante esa partida.

**RQF9.3** — El sistema debe validar el conjunto de 9 poderes contra todos los layouts predefinidos.

**RQF9.4** — El sistema debe considerar un layout "válido" solo si el conjunto de 9 poderes contiene al menos 4 poderes compatibles con las etiquetas requeridas.

**RQF9.5** — El sistema debe descartar el conjunto de 9 poderes y generar uno nuevo si algún pool de niveles queda con 0 niveles válidos.

**RQF9.6** — El sistema debe seleccionar los 9 niveles de la partida priorizando aquellos con el "Puntaje de Compatibilidad" más alto.

**RQF9.7** — El sistema debe seleccionar al azar entre niveles si existe un empate en el "Puntaje de Compatibilidad".

**RQF9.8** — El sistema debe ocultar el proceso de selección y validación detrás de una animación de "tragaperras".

**RQF9.9** — El jugador no debe poder interactuar con el juego durante la animación de "tragaperras".

**RQF9.10** — La animación de "tragaperras" debe congelarse gradualmente mostrando los 9 poderes seleccionados al terminar la selección.

### Requerimientos No Funcionales

**RQNF9.1** — El proceso de selección y validación debe completarse antes de permitir entrada del jugador.

**RQNF9.2** — El diccionario de compatibilidad debe calcularse usando el sistema de etiquetas.

**RQNF9.3** — La animación de "tragaperras" debe tener una duración mínima de 3 segundos para visualizar.

**RQNF9.4** — El sistema debe registrar el conjunto de 9 poderes seleccionados en `Logic_Manager.mazoJugador`.

**RQNF9.5** — El sistema debe guardar el conjunto de 9 niveles en `LevelLoader.nivelesDeEstaPartida`.

---

## MÓDULO 10: SISTEMA DE RE-ROLL Y LOCK

### Requerimientos Funcionales

**RQF10.1** — El sistema debe activarse únicamente en la pantalla de selección de poderes antes de iniciar un intento de nivel.

**RQF10.2** — El sistema debe asignar aleatoriamente 3 poderes iniciales (sin repetir) de los 9 disponibles para la partida.

**RQF10.3** — El jugador debe poder navegar entre los 3 poderes seleccionados usando las teclas `←` y `→`.

**RQF10.4** — El jugador debe poder "bloquear" (Lock) un poder seleccionado usando la tecla `X`.

**RQF10.5** — El sistema debe impedir que los poderes bloqueados sean modificados por un re-roll.

**RQF10.6** — El jugador debe poder "re-rolear" (Re-roll) los poderes no bloqueados usando la tecla `R`, consumiendo un uso de re-roll.

**RQF10.7** — El jugador debe poder iniciar el intento con los 3 poderes seleccionados usando la tecla `Z`.

**RQF10.8** — El sistema debe otorgar al jugador 11 "intentos" por nivel.

**RQF10.9** — El sistema debe declarar una pérdida (regreso al menú) si los "intentos" llegan a 0.

**RQF10.10** — El sistema debe reiniciar los "intentos" a 11 al pasar al siguiente nivel.

**RQF10.11** — El sistema debe otorgar al jugador 2 "re-rolls" por intento.

**RQF10.12** — El sistema debe reiniciar los "re-rolls" a 2 en el siguiente intento o al pasar al siguiente nivel.

**RQF10.13** — El sistema debe mantener los poderes bloqueados (Lock) entre intentos del mismo nivel.

**RQF10.14** — El sistema debe quitar todos los "Lock" al avanzar al siguiente nivel.

**RQF10.15** — El sistema debe mostrar información de intentos restantes y re-rolls disponibles.

**RQF10.16** — En estado "issue", el sistema debe aumentar la probabilidad de aparición de los 3 poderes con los que el jugador más ha ganado.

**RQF10.17** — En estado "solution", el sistema debe reducir en 25% la probabilidad de aparición de los 3 poderes más ganadores.

**RQF10.18** — En estado "solution", el sistema debe aumentar en 10% la probabilidad de aparición de los 3 poderes con los que el jugador menos ha ganado.

**RQF10.19** — El sistema debe realizar un desempate azaroso si existe un empate en el conteo de usos de poderes para los estados "issue" o "solution".

### Requerimientos No Funcionales

**RQNF10.1** — Los contadores de intentos y re-rolls deben ser enteros no negativos.

**RQNF10.2** — El estado de "Lock" debe ser persistente en un array booleano de tamaño 3.

**RQNF10.3** — La selección de poderes debe usar `Random.Range()` con exclusión de ya seleccionados.

**RQNF10.4** — El sistema debe mostrar visualmente cuáles poderes están bloqueados.

**RQNF10.5** — Las probabilidades ajustadas deben usar pesos (weights) normalizados.

**RQNF10.6** — El re-roll debe ser instantáneo sin animación de espera.

---

## MÓDULO 11: PROGRESIÓN POR FASES Y CONDICIONES

### Requerimientos Funcionales

**RQF11.1** — El sistema debe utilizar layouts de nivel prefabricados, no generados aleatoriamente.

**RQF11.2** — El sistema debe disponer de 15 layouts predefinidos para los niveles del 1 al 3.

**RQF11.3** — El sistema debe disponer de 15 layouts predefinidos para los niveles 4 al 6.

**RQF11.4** — El sistema debe disponer de 9 layouts predefinidos para los niveles 7 y 8.

**RQF11.5** — El sistema debe disponer de 6 layouts predefinidos para el nivel 9.

**RQF11.6** — El sistema debe dividir cada conjunto de niveles en tres fases de dificultad (fácil, neutra, difícil) basadas en el algoritmo Skill.

**RQF11.7** — El sistema debe asignar al jugador a una fase para el siguiente grupo de niveles basándose en el "Skill score".

**RQF11.8** — El algoritmo Skill debe determinar la fase basándose en la cantidad de intentos, poderes utilizados y tiempo del nivel anterior.

**RQF11.9** — El sistema debe asegurar que cada fase contiene la misma cantidad de layouts.

**RQF11.10** — Los layouts dentro de diferentes fases deben tener variaciones en complejidad, cantidad de obstáculos y distribución de elementos.

**RQF11.11** — El sistema debe mantener el objetivo del jugador como tomar coleccionables y llegar a la salida en todos los niveles.

**RQF11.12** — El sistema debe introducir "Condiciones" (modificadores aleatorios) en los niveles 4-6.

**RQF11.13** — El sistema debe mostrar las Condiciones activas en la pantalla de re-roll y lock.

**RQF11.14** — El sistema debe implementar la condición "Con las reglas de la casa" que obliga al jugador a pasar sin poder cambiar sus poderes.

**RQF11.15** — El sistema debe implementar la condición "X poder lo debes tener" que fija un poder aleatorio con lock obligatorio.

**RQF11.16** — El sistema debe implementar la condición "el monitor está fallando..." que oculta la identidad de los poderes.

**RQF11.17** — El sistema debe implementar la condición "no te acabes por completo los poderes" que requiere dejar uso restante.

**RQF11.18** — El sistema debe escalar la probabilidad de aparición de Condiciones con el "Skill score" (0% si <60, 10% a 60, hasta 50% a 100).

### Requerimientos No Funcionales de Niveles 1-3

**RQNF11.1** — Los niveles 1-3 deben usar una cámara fija de 16×29 celdas.

**RQNF11.2** — Fase "issue": 13% coleccionables, 0% frágiles, 29% flotantes.

**RQNF11.3** — Fase "based": 12% coleccionables, 3% frágiles, 31% flotantes.

**RQNF11.4** — Fase "solution": 11% coleccionables, 5% frágiles, 32% flotantes.

**RQNF11.5** — Todos los niveles 1-3: 11% estáticos, 44% enganche.

### Requerimientos No Funcionales de Niveles 4-6

**RQNF11.6** — Los niveles 4-6 deben usar una cámara de 16×29 celdas.

**RQNF11.7** — Fase "issue": 13% coleccionables, 0% frágiles, 29% flotantes.

**RQNF11.8** — Fase "based": 12% coleccionables, 3% frágiles, 31% flotantes.

**RQNF11.9** — Fase "solution": 11% coleccionables, 5% frágiles, 32% flotantes.

**RQNF11.10** — Las condiciones pueden aparecer en niveles 4-6 según la probabilidad de skill.

### Requerimientos No Funcionales de Niveles 7-8

**RQNF11.11** — Los niveles 7-8 deben usar una cámara de 16×41 celdas.

**RQNF11.12** — La cámara debe moverse únicamente de manera horizontal, manteniendo al jugador en el foco central.

**RQNF11.13** — Nivel 7-8: 11% estáticos, 37% enganche, 17% coleccionables.

**RQNF11.14** — Fase "issue": 3% frágiles, 32% flotantes.

**RQNF11.15** — Fase "based": 4% frágiles, 31% flotantes.

**RQNF11.16** — Fase "solution": 6% frágiles, 30% flotantes.

### Requerimientos No Funcionales de Nivel 9

**RQNF11.17** — El nivel 9 debe usar una cámara de 16×53 celdas.

**RQNF11.18** — La cámara debe moverse únicamente de manera horizontal, manteniendo al jugador en el foco central.

**RQNF11.19** — Nivel 9: 11% estáticos, 22% enganche, 22% coleccionables.

**RQNF11.20** — Fase "issue": 8% frágiles, 35% flotantes.

**RQNF11.21** — Fase "based": 9% frágiles, 36% flotantes.

**RQNF11.22** — Fase "solution": 10% frágiles, 37% flotantes.

### Requerimientos No Funcionales - Cajas de Poderes

**RQNF11.23** — Los niveles 7-8 deben incluir 3 "cajas de poderes".

**RQNF11.24** — El nivel 9 debe incluir 6 "cajas de poderes".

**RQNF11.25** — Las cajas deben contener poderes de los 9 seleccionados al inicio.

**RQNF11.26** — Al tocar una caja, el poder se intercambia con el seleccionado actualmente.

**RQNF11.27** — Al perder un intento, los poderes en las cajas deben cambiar aleatoriamente.

---

## MÓDULO 12: MECANICA DE CAJAS DE PODERES

### Requerimientos Funcionales

**RQF12.1** — El sistema debe incluir 3 "cajas de poderes" en los niveles 7 y 8.

**RQF12.2** — El sistema debe incluir 6 "cajas de poderes" en el nivel 9.

**RQF12.3** — Las "cajas de poderes" deben contener un poder de los 9 elegidos al inicio de la partida.

**RQF12.4** — El jugador debe poder intercambiar su poder seleccionado actualmente con el poder contenido en la caja al tocarla.

**RQF12.5** — El sistema debe cambiar aleatoriamente el poder contenido en la caja si el jugador pierde el intento.

**RQF12.6** — El jugador debe poder usar todos sus poderes a través de las cajas en el nivel 9.

### Requerimientos No Funcionales

**RQNF12.1** — Las cajas deben ser GameObjects con collider trigger.

**RQNF12.2** — El intercambio de poderes debe ser instantáneo sin animación.

**RQNF12.3** — El sistema debe evitar cajas con poderes duplicados en el mismo nivel.

---

## MÓDULO 13: SISTEMA DE TEMPORIZACIÓN Y PUNTUACIÓN FINAL

### Requerimientos Funcionales

**RQF13.1** — El sistema debe iniciar un temporizador global de partida cuando el jugador inicie el nivel 1.

**RQF13.2** — El temporizador global debe detenerse permanentemente cuando el jugador complete el Nivel 9.

**RQF13.3** — El tiempo transcurrido en la pantalla de "Re-roll y Lock" debe incluirse en el tiempo total de la partida.

**RQF13.4** — El temporizador global solo debe pausarse cuando el jugador se encuentre en el menú de pausa.

**RQF13.5** — El sistema debe acumular un "Puntaje Total" a lo largo de los 9 niveles.

**RQF13.6** — El sistema debe otorgar +3 puntos si se completó el nivel en el primer intento.

**RQF13.7** — El sistema debe otorgar +2 puntos si se completó el nivel en el segundo o tercer intento.

**RQF13.8** — El sistema debe otorgar +1 punto si se completó el nivel en el cuarto o quinto intento.

**RQF13.9** — El sistema debe otorgar un bono de +1 punto si el nivel se completa en menos de 30 segundos.

**RQF13.10** — El sistema debe otorgar un bono de +4 puntos si el nivel se completa sin gastar más de un poder.

**RQF13.11** — El sistema debe otorgar un bono de +1 punto si un nivel se completa con una "Condición" activa.

**RQF13.12** — El sistema debe mostrar una pantalla de resumen final al completar el Nivel 9.

**RQF13.13** — La pantalla final debe mostrar el "Tiempo Total", "Mejor Tiempo Guardado", "Puntaje Total" y "Mejor Puntaje Guardado".

**RQF13.14** — El sistema debe actualizar el "Mejor Tiempo Guardado" si el "Tiempo Total" de la partida actual es menor.

**RQF13.15** — El sistema debe actualizar el "Mejor Puntaje Guardado" si el "Puntaje Total" de la partida actual es mayor.

**RQF13.16** — El sistema debe mantener un registro separado de "Mejor tiempo" y "Mejor puntaje" para el "Modo Difícil".

**RQF13.17** — El sistema debe mostrar una pantalla final al perder en modo "Random crudo" indicando "Cámaras completadas totales" y "Mejor puntaje".

**RQF13.18** — El sistema debe actualizar el "Mejor Puntaje de Cámaras" si las "Cámaras Completadas Totales" son mayores.

**RQF13.19** — El sistema debe mostrar mensajes de hitos al jugador para justificar los puntos de bono obtenidos.

### Requerimientos No Funcionales

**RQNF13.1** — El temporizador debe estar en formato HH:MM:SS.

**RQNF13.2** — El puntaje total debe ser un entero no negativo.

**RQNF13.3** — El sistema debe usar `PlayerPrefs` o similar para guardar mejores tiempos y puntajes.

**RQNF13.4** — Los bonos deben sumarse de forma clara y transparente.

**RQNF13.5** — El tiempo debe ser preciso usando `Time.deltaTime` acumulado.

**RQNF13.6** — Diferentes modos (Normal, Difícil, Random Crudo) deben tener registros separados.

---

## MÓDULO 14: APÉNDICE DE PODERES

### Requerimientos Funcionales - Poder: Plataforma Estática

**RQF14.1** — El sistema debe implementar el poder "Plataforma Estática" que crea una plataforma "enganche" en el aire.

**RQF14.2** — El poder "Plataforma Estática" debe tener 3 usos.

**RQF14.3** — El poder debe crear una plataforma de tamaño 1×1 celdas.

### Requerimientos Funcionales - Poder: Impulso

**RQF14.4** — El sistema debe implementar el poder "Impulso" que genera un impulso en la dirección apuntada.

**RQF14.5** — El poder "Impulso" debe tener 5 usos.

**RQF14.6** — El poder debe mover 6 celdas a objetos dentro de un rango de 3×3 celdas.

**RQF14.7** — El poder debe mover 2 celdas a objetos dentro de un rango de 5×5 celdas.

**RQF14.8** — El poder debe mover 1 celda a objetos fuera de un rango de 5×5 celdas.

### Requerimientos Funcionales - Poder: G-Inversor

**RQF14.9** — El sistema debe implementar el poder "G-Inversor" que invierte la gravedad del jugador.

**RQF14.10** — El poder debe limitarse a un medidor de energía mientras se mantiene presionado.

**RQF14.11** — El poder debe ser continuo (no de varios usos discretos).

### Requerimientos Funcionales - Poder: EmbestiFresa

**RQF14.12** — El sistema debe implementar el poder "EmbestiFresa" que permite un "dash" en cualquier dirección.

**RQF14.13** — El poder debe tener 3 usos (aunque el código muestra 5).

**RQF14.14** — El poder debe permitir movimiento en 8 direcciones.

### Requerimientos Funcionales - Poder: Mochila de Cocas

**RQF14.15** — El sistema debe implementar el poder "Mochila de Cocas" que hace descender lentamente al jugador.

**RQF14.16** — El poder debe estar limitado por un medidor de energía.

**RQF14.17** — El poder debe ser continuo mientras se mantiene presionado.

### Requerimientos Funcionales - Poder: Coca Agitada

**RQF14.18** — El sistema debe implementar el poder "Coca Agitada" que da un impulso vertical lento.

**RQF14.19** — El poder debe permitir movimiento lateral durante el impulso.

**RQF14.20** — El poder debe tener 3 usos.

**RQF14.21** — El poder debe recorrer 5 celdas de distancia.

**RQF14.22** — El poder debe frenar en seco al jugador si está cayendo y elevarlo desde el punto de uso.

### Requerimientos Funcionales - Poder: Desplazador

**RQF14.23** — El sistema debe implementar el poder "Desplazador" que crea un área que mueve objetos.

**RQF14.24** — El poder debe tener un solo uso.

**RQF14.25** — El poder debe anular el efecto de los poderes "dash", "mochila", "coca agitada" y "g-inversor" dentro de su área.

### Requerimientos Funcionales - Poder: Cubo Repulsor

**RQF14.26** — El sistema debe implementar el poder "Cubo Repulsor" que coloca un cubo repulsor.

**RQF14.27** — El poder debe tener un solo uso.

**RQF14.28** — El cubo debe repeler al jugador al tocarlo.

### Requerimientos Funcionales - Poder: Gel Adherente

**RQF14.29** — El sistema debe implementar el poder "Gel Adherente" que permite pintar una superficie vertical.

**RQF14.30** — El poder debe permitir caminar sobre ella temporalmente.

**RQF14.31** — El poder debe tener 4 usos.

**RQF14.32** — El gel debe durar 10 segundos después de su uso.

### Requerimientos Funcionales - Poder: Paso Sombra

**RQF14.33** — El sistema debe implementar el poder "Paso Sombra" que marca la posición actual.

**RQF14.34** — El poder debe permitir regresar a esa posición una vez.

**RQF14.35** — El poder debe tener un solo uso.

**RQF14.36** — El sistema debe permitir activar "Paso Sombra" presionando la tecla de usar poder o saliéndose de los límites.

### Requerimientos Funcionales - Poder: Imán

**RQF14.37** — El sistema debe implementar el poder "Imán" que lanza un imán que atrae plataformas.

**RQF14.38** — El poder debe tener 3 usos.

**RQF14.39** — El poder debe recorrer una distancia de 6 celdas a menos que haya una plataforma compatible.

**RQF14.40** — El poder no debe gastar un uso si toca una plataforma "estático" o si no toca nada.

**RQF14.41** — El poder debe convertir una plataforma de "enganche" en "flotante" al tocarla.

**RQF14.42** — El poder debe atraer plataformas "enganche", "desenganche" y "flotantes".

### Requerimientos Funcionales - Poder: Reseteo Local

**RQF14.43** — El sistema debe implementar el poder "Reseteo Local" que devuelve el layout a su estado inicial.

**RQF14.44** — El poder debe tener un solo uso.

**RQF14.45** — El poder no debe restaurar los usos de otros poderes.

### Requerimientos Funcionales - Poder: Error de Código

**RQF14.46** — El sistema debe implementar el poder "Error de Código" que intercambia la posición de salida y entrada.

**RQF14.47** — El poder debe tener un solo uso.

### Requerimientos Funcionales - Poder: Brújula Gravitacional

**RQF14.48** — El sistema debe implementar el poder "Brújula Gravitacional" que cambia la gravedad global.

**RQF14.49** — El poder debe tener un solo uso.

**RQF14.50** — El poder debe elevar al jugador hacia la dirección contraria.

**RQF14.51** — El poder debe elevar al jugador hasta tocar los límites o una plataforma.

**RQF14.52** — El poder debe hacer aparecer una flecha en la dirección a la que el jugador apunta.

**RQF14.53** — El poder debe continuar elevando al jugador en niveles donde la cámara se mueve.

**RQF14.54** — El sistema debe asegurar al jugador de no morir si sale fuera de los límites con la gravedad cambiada.

### Requerimientos Funcionales - Poder: Singularidad

**RQF14.55** — El sistema debe implementar el poder "Singularidad" que elimina objetos en un área designada.

**RQF14.56** — El poder debe tener 3 usos.

**RQF14.57** — El poder debe mostrar una "X" a 3 celdas de distancia del jugador en la dirección apuntada.

**RQF14.58** — El sistema debe asegurar que el poder solo apunta en 4 puntos cardinales.

**RQF14.59** — El poder debe eliminar todo en un área de 2×2 celdas encima de la "X".

**RQF14.60** — El poder no debe eliminar la salida ni la entrada con sus plataformas.

### Requerimientos No Funcionales - Poderes

**RQNF14.1** — Todos los poderes deben tener un componente de script en el jugador.

**RQNF14.2** — Los poderes deben registrar su uso en `SkillManager.RegistraUsoPoder()`.

**RQNF14.3** — Los poderes deben respetar `Time.timeScale` para no funcionar en pausa.

**RQNF14.4** — El contador de cargas debe ser accesible por reflexión mediante campo `cargasRestantes`.

**RQNF14.5** — Los poderes de dirección deben soportar 8 direcciones usando input de ejes.

**RQNF14.6** — Los poderes continuos deben usar corrutinas para manejar duraciones.

**RQNF14.7** — El sistema debe contar como derrota salir de los bordes incluso con la gravedad invertida.

**RQNF14.8** — El sistema debe reaparecer al jugador en la plataforma de entrada si pierde (con gravedad inicial).

**RQNF14.9** — Las plataformas deben ser afectadas por cambios de gravedad globales.

---

## MÓDULO 15: PAUSA Y CONTROL DE TIEMPO

### Requerimientos Funcionales

**RQF15.1** — El jugador debe pausar y reanudar el juego con la tecla `ESC`.

**RQF15.2** — El sistema debe mostrar un menú de pausa cuando está pausado.

**RQF15.3** — El sistema debe permitir reiniciar el nivel desde la pausa con la tecla `R`.

**RQF15.4** — El sistema debe ocultar el menú de pausa al reanudar.

### Requerimientos No Funcionales

**RQNF15.1** — La pausa debe establecer `Time.timeScale = 0`.

**RQNF15.2** — La reanudación debe establecer `Time.timeScale = 1`.

**RQNF15.3** — El menú de pausa debe estar en una escena o canvas separado.

**RQNF15.4** — El reinicio debe recargar la escena actual sin cambiar de nivel.

---

## MÓDULO 16: ARQUITECTURA, PERSISTENCIA Y CALIDAD

### Requerimientos No Funcionales de Arquitectura

**RQNF16.1** — El sistema debe preservar entre escenas los gestores de estado: `SkillManager`, `LevelLoader`, `Logic_Manager` y `CondicionesManager` usando `DontDestroyOnLoad()`.

**RQNF16.2** — El sistema debe usar `ScriptableObject` para modelar datos de poderes (`PoderData`) y niveles (`LevelData`).

**RQNF16.3** — El sistema debe implementar el patrón Singleton para gestores globales.

**RQNF16.4** — El sistema debe clasificar niveles y poderes mediante etiquetas y utilizar un diccionario de relaciones.

**RQNF16.5** — El sistema debe manejar hasta 9 poderes visibles y al menos 15 poderes posibles en la baraja de inicio.

**RQNF16.6** — La selección de poderes debe usar listas genéricas de `PoderData`.

**RQNF16.7** — Los scripts deben usar namespaces para organizar la lógica (ej: `MONOLISA2.Powers`, `MONOLISA2.UI`).

### Requerimientos No Funcionales de Manejo de Errores

**RQNF16.8** — El sistema debe manejar referencias nulas gracefully con `?.` operator o validaciones explícitas.

**RQNF16.9** — El sistema debe registrar errores en consola cuando falten componentes criticamente.

**RQNF16.10** — El sistema debe validar que todos los managers estén presentes al iniciar el juego.

**RQNF16.11** — El sistema debe evitar bloqueos del juego cuando falta un recurso (usar valores por defecto).

### Requerimientos No Funcionales de Validación

**RQNF16.12** — Todos los valores numéricos deben estar acotados (ej: skill entre 0 y 100, intentos ≥ 0).

**RQNF16.13** — El sistema debe validar que el diccionario de relaciones contenga todas las etiquetas implementadas.

**RQNF16.14** — El sistema debe impedir valores negativos en contadores de intentos, tiempo y poderes.

**RQNF16.15** — El sistema debe evitar división por cero en cálculos de probabilidades y puntajes.

### Requerimientos No Funcionales de Usabilidad

**RQNF16.16** — El sistema debe usar controles de teclado simples y predecibles.

**RQNF16.17** — El sistema debe mostrar retroalimentación visual clara del poder seleccionado, usos restantes, nivel actual y condiciones activas.

**RQNF16.18** — El sistema debe ser robusto frente a referencias nulas o activos faltantes.

**RQNF16.19** — El sistema debe ser accesible para plataformas de escritorio Windows y Mac.

**RQNF16.20** — El sistema debe ejecutarse sin requerir hardware de alto rendimiento.

### Requerimientos No Funcionales de Compatibilidad

**RQNF16.21** — El proyecto debe compilar sin errores en Unity 2022.3 LTS o superior.

**RQNF16.22** — El proyecto debe usar C# 9.0 o superior.

**RQNF16.23** — El proyecto debe ser compatible con URP (Universal Render Pipeline).

**RQNF16.24** — El proyecto debe soportar resolución mínima de 1280×720 píxeles.

### Requerimientos No Funcionales de Performance

**RQNF16.25** — El juego debe ejecutarse a 60 FPS como mínimo en una computadora promedio.

**RQNF16.26** — El tiempo de carga de niveles debe ser menor a 2 segundos.

**RQNF16.27** — El sistema de física debe usar `FixedUpdate()` para simulación consistente.

**RQNF16.28** — El sistema debe usar pooling de objetos para poderes que generan instancias (proyectiles, plataformas).

### Requerimientos No Funcionales de Documentación

**RQNF16.29** — Todos los scripts deben incluir comentarios XML para documentación de métodos públicos.

**RQNF16.30** — El código debe seguir las convenciones de nomenclatura de C# (PascalCase para clases, camelCase para variables).

**RQNF16.31** — Las funciones complejas deben incluir comentarios explicativos inline.

**RQNF16.32** — El proyecto debe mantener un archivo README.md con instrucciones de configuración.

---

## RESUMEN

**Total de Requerimientos Funcionales (RQF):** 226+ (incluidos los del documento original + agregados)

**Total de Requerimientos No Funcionales (RQNF):** 100+ (detallados por módulo de arquitectura, validación, performance, usabilidad y compatibilidad)

**Total General:** 326+ requerimientos consolidados y validados

---

## VALIDACIÓN VERSUS CÓDIGO FUENTE

### Validaciones Exitosas
- ✅ Sistema de poderes: 15 poderes implementados coinciden con la especificación
- ✅ Etiquetas: Sistema de relaciones en `LogicaPoderes.Relaciones` implementado
- ✅ HUD: `PowerSelector` y `PowerSelectorPrincipal` coinciden con especificación
- ✅ Skill: `SkillManager` implementa algoritmo correctamente
- ✅ Re-roll y Lock: `ReRollManager` implementa sistema completo
- ✅ Plataformas: `Bloque_Fragil`, `Bloque_Flotante` implementadas correctamente
- ✅ Pausa: `ControlarPausaMenu` implementa control de `Time.timeScale`

### Inconsistencias Encontradas
- ⚠️ `EmbestifresaPower` muestra 5 cargas en código pero requerimiento especifica 3
- ⚠️ Algunos poderes no tienen documentación de etiquetas completa
- ⚠️ Falta implementación de algunas condiciones específicas (Monitor Fallando)

### Elementos a Completar
- 🔲 Implementación de todas las cajas de poderes (7-8 y 9)
- 🔲 Modo "Random Crudo" completamente funcional
- 🔲 Animación de tragaperras en selección inicial
- 🔲 Sistema de guardar mejor tiempo y puntaje en persistencia

