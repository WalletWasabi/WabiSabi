# Scheduling

## Overview

A CoinJoin round traditionally consists of 3 main Phases: Input Registration, Output Registration and Transaction Signing. While this is not the complete protocol, we will start out describing how a system can work with these Phases, then identify issues and extend the protocol by resolving these issues. Finally we present the whole protocol in full.

![](https://camo.githubusercontent.com/f791b707db282271021f5a5421c2334a8cdfd7d5/68747470733a2f2f692e696d6775722e636f6d2f54304677695a682e706e67)

### 1. Input Registration

In Input Registration Phase Participants register their inputs to the Coordinator. When sufficient number of inputs are registered or the Phase times out the Coordinator progresses to to Output Registration Phase. At Input Registration Phase, the Coordinator does not need to note any misbehaving Participants as it can just decide to not kick off the next Phase.

```
Participant =input=> Coordinator
Participant <=ACK/NACK= Coordinator
```

### 2. Output Registration

In Output Registration Phase Participants register their outputs to the Coordinator. When the same value of outputs are registered or the Phase times out the Coordinator progresses to to Transaction Signing Phase. At Output Registration Phase, the Coordinator cannot note any misbehaving Participants as participants are identified by their inputs, instead it always progresses to Transaction Signing Phase and misbehaving Participants will be the ones those do not sign.

```
Participant =output=> Coordinator
Participant <=ACK/NACK= Coordinator
```

### 3. Transaction Signing

In Transaction Signing Phase Participants retrieve the transaction from the Coordinator, sign it, finally give their signatures to the Coordinator. When all input signatures arrived the Coordinator combine the transaction and broadcasts it to the Bitcoin network. If not all Input Signatures arrive, then the Coordinator progresses to a special round, called the Blame Round, where only inputs those have signed the transaction can register to.

```
Participant =get coinjoin=> Coordinator
Participant <=coinjoin= Coordinator

Participant =input, signature=> Coordinator
Participant <=ACK/NACK= Coordinator
```

## Problem: IP Address Fingerprinting

The problem with the above described scheme is that the Coordinator can remember the IP addresses of Participants and thus deanonymize them based on this information. To resolve it, we utilize multiple anonymity network identities: Alice, Bob, Satoshi.  

- Alices are identities who are associated with inputs.
- Bobs are identities who are associated with Outputs.
- Satoshis are monitoring identities.

```
[INPUT REGISTRATION]
Alice1 =input1=> Coordinator
Alice1 <=ACK/NACK= Coordinator

Alice2 =input2=> Coordinator
Alice2 <=ACK/NACK= Coordinator



[OUTPUT REGISTRATION]
Bob =output=> Coordinator
Bob <=ACK/NACK= Coordinator



[TRANSACTION SIGNING]
Satoshi =get coinjoin=> Coordinator
Satoshi <=coinjoin= Coordinator

Alice1 =input1, signature1=> Coordinator
Alice1 <=ACK/NACK= Coordinator

Alice2 =input2, signature2=> Coordinator
Alice2 <=ACK/NACK= Coordinator
```
