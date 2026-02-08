# Flscap Aim System

A modular, extensible aiming **interaction system** for Unity.

This package provides a small orchestration layer for handling aiming as a **modal interaction**:

- Start aiming  
- Update live data  
- Optionally visualize
- Commit intent  

The system is deliberately **unopinionated** about gameplay, abilities, and data meaning.  
All semantics are owned by the consumer.

---

## Core Concepts

### AimSystem

`AimSystem` is the orchestrator.

It coordinates the lifecycle of an aiming interaction, but does **not** interpret or validate aim data.

**Responsibilities:**
- Manage the active aim mode
- Update it every frame
- Spawn and clean up an optional visualizer
- Commit the aim when requested

**It does not:**
- Know what aiming means
- Know what the data represents
- Route abilities
- Broadcast global events

---

### AimRequest

An `AimRequest` represents **why** the player is aiming and **what should happen** when the aim is committed.

```csharp
var request = new AimRequest<AbilityContext, DirectionalAimData>(
    new AbilityContext { Name = "Fireball" },
    (context, result) =>
    {
        FireAbility(context, result);
    }
);
```

**Key properties:**
- Fully type-safe at the call site
- No casting required in gameplay code
- Carries optional contextual data
- Encapsulates the continuation (`onCommitted` callback)

Runtime validation ensures that mismatched aim data types fail **loudly and explicitly**.

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
- They maintain and update aim data over time
- They decide when the data is valid
- They return the *same data object* on commit

For visualization and inspection, a typed extension is available:

```csharp
public interface IAimMode<TAimData> : IAimMode
{
    TAimData Data { get; }
}
```

---

### Aim Data Model

This system intentionally uses **a single data type** for both:
- live, mutable aiming data
- final, committed aim data

The distinction is **temporal**, not structural.

- During aiming: the data is mutable and incomplete
- On commit: the data is treated as final and valid

This avoids artificial type splits, runtime casts, and pairing errors while keeping the API honest and simple.

Immutability after commit is a **documented convention**, not a forced constraint.

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
    : AimVisualizer<IAimMode<DirectionalAimData>>
{
    // draw preview using aim data
}
```

Visualizers are expected to be specific and are not part of the generic infrastructure.

---

## Example Flow

```csharp
// Create providers
var originProvider = new TopDownTransformOriginProvider(transform);
var directionProvider = new TopDownMouseDirectionProvider(Camera);

// Create aim mode
var aimMode = new FixedRangeDirectionalAimMode(
    originProvider,
    directionProvider,
    range: 10f
);

// Create request
var request = new AimRequest<AbilityContext, DirectionalAimData>(
    new AbilityContext { Name = "Test Ability" },
    (context, result) =>
    {
        Debug.Log(
            $"Ability: {context.Name}, " +
            $"Origin: {result.Origin}, " +
            $"Direction: {result.Direction}, " +
            $"Range: {result.Range}"
        );
    }
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
  Strong typing where gameplay code is written; type erasure only at orchestration boundaries.

- **User-owned data**  
  No framework-defined aim data or result classes.

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

If you enjoy building systems, clean APIs, and composable interactions, this package is for you.
