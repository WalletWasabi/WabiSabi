# Collaborative Transactions Protocol

Generalization of [Chaumian CoinJoins](https://github.com/nopara73/ZeroLink/).

## Overview

The protocol consists of epochs, rounds and phases.

![](https://i.imgur.com/ElMcN27.png)

**Input Registration Phase**, **Payment Registration Phase** and **Signing Phase** follow each other and together they are called the **Main Round**.

**Input Registration Phase** cannot fail, because inputs can be refused or kicked out of the round without penalty before moving on to **Payment Registration Phase**. Regardless if **Payment Registration Phase** succeeds or fails, the round must progress to **Signing Phase**, because penalties can only be imposed on malicious inputs and not on malicious outputs. If **Signing Phase** fails, the protocol progresses to **Blame Round**, which repeats the **Main Round** without the malicious inputs.

The **Main Round** and the potential **Blame Rounds** are called an **Epoch**.

#### Phase 1: Input Registration

Peers register their inputs to the coordinator. Inputs must be registered separately through different anonymtiy network identities, unless an input's amount does not reach the minimum required amount, in which case the input can be registered together with other inputs.

The peer provides a set of inputs along with corresponding proof of ownerships and blinded tokens. These inputs must be unspent, confirmed and mature. The blinded tokens are blinded random data. These tokens must be created as follows:

- If the input sum is 10BTC, then the peer creates 10 tokens and blinds all of them for 1BTC.
- If the input sum is 100BTC, then the peer creates 10 tokens and blinds all of them for 10BTC.
- If the input sum is 90BTC, then the peer creates 90 tokens and blinds all of them for 1BTC.
- If the input sum is 990BTC, then the peer creates 180 tokens and blinds 90 of them for 10BTC and 90 of them for 1BTC.

The coordinator blindly signs the tokens and responds with the signatures.

#### Phase 2: Payment Registration

Every payment registration request must happen through different anonymity network identities.

Peers are allowed to utilize their signed tokens in any way they would like to. A peer can decide to take 3 1BTC token and create a 0.5BTC and 2.5BTC outputs. Thus arbitrary payments are possible.

However to enhanche privacy peers are recommended to register payments of 1, 10, 100, etc... satoshi payments as follows:

- If peer has 9 1BTC tokens, then register them separately for 1BTC outputs.
- If peer has 10 1BTC tokens, then register them together for a 10BTC output.
- If peer has 11 1BTC tokens, then register 10 of them together for a 10BTC output and separately the remaining one for a 1BTC output.
- If peer has 11 10BTC tokens and 2 1BTC tokens then register 10 10BTC token together for a 100BTC output, and separately 1 10BTC token for a 10BTC output and the remaining 2 1BTC tokens each separately for 1BTC outptus.

#### Phase 3: Signing

The final transaction is given out to the peers for signing.  

If an input from an input group did not sign the transaction, then the coordinator prohibits these inputs from participating in followup epochs and the current epoch progresses to a **Blame Round** where only the current epoch's honest peers are allowed to participate.

## Research Questions

- Maximum number of tokens to be signed - computational bottleneck.
- Maximum number of tokens to be signed - network bottleneck.
- Maximum number of tokens to be signed - security bottleneck (Wagner attack.)
- Minimum required amount.
- Wagner attack.
- DoS attack.
- Sybil attack.
- Reducing network trafic by utilizing hierarchical deterministic signing key generation.
- Ensuring integrity of blind signing key - https://github.com/cashshuffle/spec/issues/22
- Scheduling.
- Timing attack.
- Preferred number series vs power of 2 vs power of 10.
- Allowing unconfirmed inputs to be registered.
- Network fees.
- Coordinator fees.
- Randomization to increase ureliability of heuristics.
- Privacy guarantees.
- UX.
- Small vs fast rounds.
