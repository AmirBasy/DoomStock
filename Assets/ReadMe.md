# Progetto DoomStock
## Prossima Release

----------------------------
- v.0.4
---------------------------- 
- Nascita popolani: nascono ad ogni evento "nascita". Unità necessarie per scatenare evento nascita impostabile da editor.
- La durata della vita di un popolano è scelta alla nascita. E' un valore random tra range preimpostato.
- Morte popolano: muore quando raggiunge il limite di età o quando deve cibarsi e non c'è più risoresa cibo disponibile.
- Nutrizione popolano: ad ogni evento "eat" mangiano se il cibo è sufficiente, altrimenti muoiono.

- Menù "pozza" (colore arancione) si apre con tasto separato (!!!) e lo può aprire solo il player che è "attivo" sulla cella pozza e può scegliere dalla lista dei popolani non assegnati di assegnarlo ad un suo edificio.
- Menù player: menù contestuale player:
-- Se la cella è vuota si può costruire uno degli edifici propri del player.
-- Se la cella è occupata da un edificio di proprietà del player lo puoi rimuovere con produzione di "macerie".
-- Se la cella è occupata da un edificio di proprietà del player si può aggiungere un popolano dall'edificio o rimuoverne uno se presente.
-- Se la cella è occupata da un edificio di proprietà di un altro player si possono vedere le info (al momento non ci sono info visualizzate).

- Influenza della produzione dei buildings sulle risorse:
-- [P1] Farm -> aumento cibo (se c'è almeno un popolano)
-- [P2] Estrattore Stone -> aumento pietra se c'è almeno un popolano (produzione continua).
-- [P2] Estrattore Wood -> aumento legna se c'è almeno un popolano (produzione continua).
-- [P3] Chiesa -> ogni chiesa aumenta di 1 l'healthcare senza necessità di avere un popolano all'interno dell'edificio. 
-- [P3] Chiesa -> se sono presenti popolani viene aumentata la risorsa fede (produzione continua).
-- [P4] Torretta -> Fa aumentare il morale se c'è un popolano dentro (produzione continua).

- L'happiness aumenta di 1 ogni volta che un popolano viene messo nell'edificio corrispondente alla sua ambizione.
- Macerie: non consentono di costruire un edificio. possono essere rimosse solo dal [p2] che con questa operazione recupera 1/4 del materiale usato per costruire l'edificio.
- Terreno di debug generato ha hightmap.
- Griglia con altezze lette da hightmap.
- Le zone non percorribili (colore grigio) e non edificabili sono generate da una hightmap.
- Gli edifici hanno un tempo di produzione prima di diventare disponibili per aggiungere popolani.
- Gli edifici "in produzione" (con almeno un popolano dentro) hanno un'animazione.
- Contatore degli anni e mesi.
- Il contatori della risorsa cibo assumono colore giallo quando non sufficiente a sfamare tutti i popolano in gioco e rosso quando al turno precedente di "eat" è morto un popolano per mancanza cibo.

- Segmaposti per eventi. Vengono visualizzati con un segnaposto quando avvengono eventi specifici:
-- Nascita popolano
-- Morte popolano
-- Spostamento popolano
-- Costruzione edificio
-- ternmine costruzione edificio
-- rimozione edificio

---- 
Bug conosciuti:
- Quando un popolano muore continua a produrre.
- Healthcare non viene incrementata.
- se un edificio produce più risorse, vengono incrementate tutte contemporaneamente a ogni evento "aumenta risorsa"
- tasto conferma su un menu vuoto dà errore
- rimozione edificio durante la costruzione non funziona.
---

v.0.0.2
- Movimento dei player sulla griglia

- Possibilità per i player di creare edifici di loro pertinenza

- Possibilità di aggiungere popolazione all'edificio corrente per generare risorse

- Creazione di eventi principali: Nascita di un popolano, produzione di risorse, Fine Anno, Fine Mese, Degrado degli edifici.

###### Note
 Player 1
                 Up = KeyCode.W,
                 Left = KeyCode.A,
                 Down = KeyCode.S,
                 Right = KeyCode.D,
                 AddBuilding = KeyCode.Z,
                 AddPopulation = KeyCode.X,
                 RemovePopulation = KeyCode.E,
                    
    
 Player 2
                Up = KeyCode.I,
                Left = KeyCode.J,
                Down = KeyCode.K,
                Right = KeyCode.L,
                AddBuilding = KeyCode.N,
                AddPopulation = KeyCode.U,
                RemovePopulation = KeyCode.O,
    
Player 3
                Up = KeyCode.UpArrow,
                Left = KeyCode.LeftArrow,
                Down = KeyCode.DownArrow,
                Right = KeyCode.RightArrow,
                AddBuilding = KeyCode.F2,
                AddPopulation = KeyCode.PageUp,
                RemovePopulation = KeyCode.PageDown,

Player 4
                Up = KeyCode.Keypad8,
                Left = KeyCode.Keypad4,
                Down = KeyCode.Keypad5,
                Right = KeyCode.Keypad6,
                AddBuilding = KeyCode.F1,
                AddPopulation = KeyCode.KeypadPlus,
                RemovePopulation = KeyCode.KeypadMinus,
 
       

---
## Storico Releases

v.0.0.1
- Creazione progetto
---
