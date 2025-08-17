# Lost and Found

## Controls

### Movement:

W / ↑ – Move Forward

S / ↓ – Move Backward

A / ← – Move Left

D / → – Move Right

E - Interact

Shift + WASD - Sprint

Space – Jump

Mouse – Look Around

Left Mouse Button – Skip dialogue animation

### Mini-Games / Puzzle Controls:
Clean up the park - Pick up (Interact) all the rubbish around the park

Drag & Drop (Poster Puzzle) – Click and hold to pick up a piece, move to correct slot.

## Limitations / Bugs
Camera clipping can occur when the player stands too close to walls. 

Players cannot jump, as doing so causes them to float away.

In some missions, NPC pathfinding can get stuck on corners or narrow spaces. 

Lack of mobile optimization — controls are PC-focused. 

Some dialogue text may overlap in smaller resolutions.

## FSM Diagram & AI Implementation Details
### Uses of FSM:

Martin (Friend 1)

John (Friend 2)

Police Officer

Citizen

### States:
Idle – Standing, looking around.

Follow – Walk behind user.

Unfollow - Stop following user, lingering around.

Walking around - Walks around the map without interacting with the player.

### FSM Diagram
Citizen

<img width="470" height="175" alt="Screenshot 2025-08-15 151050" src="https://github.com/user-attachments/assets/c7f39619-9df7-41c8-a3f5-b4985f0c5018" />

Friend

<img width="470" height="175" alt="Screenshot 2025-08-17 194322" src="https://github.com/user-attachments/assets/2de0d83f-85c6-4352-a5be-c510f371f1e5" />

Police

<img width="470" height="175" alt="Screenshot 2025-08-17 194318" src="https://github.com/user-attachments/assets/7c765e2a-1bba-4919-b6e3-14daa0b9300a" />

### Implementation:
Citizen - The citizen cycles through a list of walk points, moving to each point in order. When the citizen reaches a waypoint, it idles for a specified time before moving on.
```csharp
IEnumerator Patrol()
    {
        if (walkPoints.Length == 0)
            yield break;

        while (true)
        {
            if (currentState == State.Patrol)
                agent.SetDestination(walkPoints[currentIndex].position); // Move to the current waypoint

            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                yield return null;

            currentState = State.Idle; // When the agent reaches the destination, let it idle

            if (currentState == State.Idle)
            {
                yield return new WaitForSeconds(pauseTime); // Wait for a short duration

                currentIndex++;
                if (currentIndex >= walkPoints.Length) // reset to the first waypoint
                    currentIndex = 0;
                currentState = State.Patrol;
            }
            yield return null;
        }
    }
```

Friend - The friend follows the player, when the friend reaches a specific place, it stops following and starts leaving. The friend will move toward an exit point and is destroyed when close enough.
```csharp
    void Update()
    {
        switch (currentState)
        {
            case State.Following:
                if (player != null)
                {
                    agent.SetDestination(player.position);
                }
                break;

            case State.LeftDueToPolice:
                if (exitPoint != null)
                {
                    agent.SetDestination(exitPoint.position);
                    if (Vector3.Distance(transform.position, exitPoint.position) < 3f) // Check if the friend is close to the exit point
                    {
                        Destroy(gameObject); // Destroy the friend object
                    }
                }
                break;
        }
    }
  ```
```csharp
/// <summary>
    /// Called when player visits police station - friend will leave
    /// </summary>
    public void OnPoliceStationVisited()
    {
        if (currentState == State.Following)
        {
            StartLeavingDueToPolice();
        }
    }

    /// <summary>
    /// Start the leaving process due to police station visit
    /// </summary>
    private void StartLeavingDueToPolice()
    {
        currentState = State.LeftDueToPolice;
        
        // Stop any existing dialogue
        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        
        Destroy(gameObject, 3f); // Destroy after 3 seconds to simulate leaving
        
        Debug.Log("Friend is leaving due to police station visit");
    }
```

Police - The police will idle unless the player has visited the police station. When activated, the police switches between idle and following states, following them. If the player is lost or the police is deactivated, it returns to idle.
```csharp
    void CheckPoliceStationStatus()
    {
        if (PlayerPrefs.GetInt("VisitedPoliceStation", 0) == 1)
        {
            shouldChasePlayer = true;
            Debug.Log("Police activated - Player has visited police station");
        }
        else
        {
            shouldChasePlayer = false;
            Debug.Log("Police inactive - Player hasn't visited police station yet");
        }
    }
```
```csharp
    IEnumerator FollowPlayer()
    {
        while (currentState == State.FollowPlayer  && shouldChasePlayer)
        {
            if (targetTransform != null)
            {
                myAgent.SetDestination(targetTransform.position);
            }
            else
            {
                // Lost target, go back to Idle
                SwitchState(State.Idle);
                yield break;
            }
            yield return null;
        }
    }

```


## Puzzle Answers
Poster Puzzle (From top left to bottom left to top right to bottom right):

1: B

2: G

3: D

4: E

5: C

6: A

7: F

8: H


## References & Credits
### Character Inspiration:

Thoughtful Bunch by [LTA](https://www.lta.gov.sg/content/ltagov/en/getting_around/public_transport/a_better_public_transport_experience/gracious_commuting.html)

### Fonts from Google Fonts:

Poppins - https://fonts.google.com/specimen/Poppins/license

Sound effects from Freesound – Licensed under CC0 / CC-BY

Poster puzzle images sourced from [Singapore Police Force Public Awareness Materials]

### Tutorials
Unity AUDIO MIXER and Unity AUDIO Volume Settings Menu tutorials by [Rehope Games](https://www.youtube.com/@RehopeGames)

Start Menu and Transitions tutorial by [Brackeys](https://www.youtube.com/@Brackeys)

Spawn Car Prefabs and Destroy Prefabs tutorial by [Unity Learn](https://learn.unity.com/project/unit-2-basic-gameplay?uv=6&courseId=5cf96c41edbc2a2ca6e8810f)

Dialogue System tutorial by [BMo](https://www.youtube.com/@BMoDev)
