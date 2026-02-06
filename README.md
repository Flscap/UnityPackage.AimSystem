# Flscap Aim System

A modular, extensible aiming **interaction system** for Unity.

This package provides a small orchestration layer for handling aiming as a **modal interaction**:

- Start aiming  
- Update live state  
- Optionally visualize  
- Commit intent  

The system is deliberately **unopinionated** about gameplay, data shapes, and abilities.  
All meaning is owned by the consumer.

---

## Core Concepts

### AimSystem

`AimSystem` is the orchestrator.

It coordinates the lifecycle of an aiming interaction, but does **not** interpret or process aim data.

**Responsibilities:**
- Manage the active aim mode
- Update it every frame
- Spawn and clean up an optional visualizer
- Commit the result when requested

**It does not:**
- Know what aiming means
- Know what data is produced
- Route abilities
- Broadcast global events

---

### AimRequest

An `AimRequest` represents **why** the player is aiming and **what should happen** when the aim is committed.

```csharp
var request = new AimRequest<AbilityContext, DirectionalAimResult>(
    (context, result) =>
    {
        FireAbility(context, result);
    },
    new AbilityContext { Name = "Fireball" }
);
```

**Key properties:**
- Fully type-safe at the call site
- No casting required in gameplay code
- Supports optional contextual data
- Encapsulates the continuation (`onCommitted` callback)

---

### IAimMode

An `IAimMode` encapsulates **how** aiming works.

```csharp
public interface IAimMode
{
    void UpdateState(float deltaTime);
    bool TryProjectIntent(out object aimData);
}
```

- Aim modes are user-defined
- They may maintain any internal state
- They produce a result only when committed
- The result type is fully owned by the user

For visualization, a typed extension is available:

```csharp
public interface IAimMode<TState> : IAimMode
{
    TState State { get; }
}
```

---

### AimVisualizer

Visualizers are optional, presentation-only components.

They are:
- Explicitly type-coupled to the aim mode they support
- Instantiated from prefabs
- Updated automatically while aiming
- Cleaned up when the interaction ends

Example:

```csharp
public sealed class DirectionLineAimVisualizer
    : AimVisualizer<IAimMode<DirectionalAimState>>
{
    // draw preview using typed state
}
```

Visualizers are **not** generic infrastructure and are expected to be specific.

---

## Example Flow

```csharp
// Create aim mode
var aimMode = new FixedRangeDirectionalAimMode(
    originProvider,
    directionProvider,
    range: 10f
);

// Create request
var request = new AimRequest<DirectionalAimResult, AbilityContext>(
    (context, result) =>
    {
        Debug.Log($"Ability: {context.Name}, Range: {result.Range}");
    },
    new AbilityContext { Name = "Test Ability" }
);

// Start aiming
aimSystem.StartAiming(
    request,
    aimMode,
    directionalVisualizerPrefab
);

// Later (e.g. on button press)
aimSystem.TryCommitAim(request);
```

---

## Design Principles

- **Interaction, not events**  
  Aiming is a modal interaction, not a broadcast signal.

- **Type safety at the edges**  
  Strong typing where code is written; type erasure only at orchestration boundaries.

- **User-owned data**  
  No framework-defined aim data or state classes.

- **Minimal assumptions**  
  No required origin, direction, target, or range.

- **Unity as host, not owner**  
  Core logic is plain C#; Unity is used for lifecycle and visualization.

---

## What This Package Is (and Is Not)

**This is:**
- An aiming interaction coordinator
- A foundation for abilities, tools, or input-driven actions
- Suitable for reuse across projects

**This is not:**
- A gameplay system
- A targeting solution with built-in rules
- A networking or replication framework
- A visual effects package

---

## Installation

This is a Unity Package Manager (UPM) package.

**Local installation:**

```
Packages/
└── com.flscap.aimsystem/
```

**Via Git:**

```json
"com.flscap.aimsystem": "https://github.com/yourname/aimsystem.git"
```

---

## Philosophy

> **AimSystem coordinates *when* and *how long*.  
> You decide *what it means*.**

If you enjoy building systems and clean APIs, this package is for you.