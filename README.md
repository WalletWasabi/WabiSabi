# Collaborative Transactions Protocol

Generalization of [Chaumian CoinJoins](https://github.com/nopara73/ZeroLink/).

## Overview

The protocol consists of epochs, rounds and phases.

![](https://i.imgur.com/d5mOgVG.png)

**Input Registration Phase**, **Output Registration Phase** and **Signing Phase** follow each other and together they are called the **Main Round**.

**Input Registration Phase** cannot fail, because inputs can be refused or kicked out of the round without penalty before moving on to **Output Registration Phase**. Regardless if **Output Registration Phase** succeeds or fails, the round must progress to **Signing Phase**, because penalties can only be imposed on malicious inputs and not on malicious outputs. If **Signing Phase** fails, the protocol progresses to **Blame Round**, which repeats the **Main Round** without the malicious inputs.

The **Main Round** and the potential **Blame Rounds** are called an **Epoch**.

#### Phase 1: Input Registration

Peers register their inputs to the coordinator. Inputs must be registered separately through different anonymtiy network identities, unless an input's amount does not reach the minimum required amount, in which case the input can be registered together with other inputs.

The peer provides a set of inputs along with corresponding proof of ownerships and blinded tokens. These inputs must be unspent, confirmed and mature. The blinded tokens are blinded random data. These tokens must be created as follows:

- If the input sum is 10BTC, then the peer creates 10 tokens and blinds all of them for 1BTC.
- If the input sum is 100BTC, then the peer creates 10 tokens and blinds all of them for 10BTC.
- If the input sum is 90BTC, then the peer creates 90 tokens and blinds all of them for 1BTC.
- If the input sum is 990BTC, then the peer creates 180 tokens and blinds 90 of them for 10BTC and 90 of them for 1BTC.

The coordinator blindly signs the tokens and responds with the signatures.

#### Phase 2: Payment Intent Registration

Peer registers arbitrary payment intents with his certificates.

### Examples

Peer got a 1BTC certificate and would like to send 0.1234 BTC to an address, then his payment intent would be 0.1234BTC - address and 0.8766BTC - address2.  

#### Phase 3: Signing + Blame

Transaction goes out for signing. The input that does not sign the transaction will be prohibited from further participation.

## Randomization

Randomization (along desired distributions) is crucial for number of inputs registered, certificate size requests and payment intent registrations in order to avoid patterns from emerging and as a consequence heuristics to be applied.

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
