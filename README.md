**Authors.** István András Seres, nopara73, nothingmuch
**Acknowledgement.** We thank the numerous contributors to this scheme. We will update the full list of contributors after the paper passes the work in progress status.
**Status.** Work in progress.

# WabiSabi: Trustless and Arbitrary CoinJoins

**Abstract.** Generalization of [Chaumian CoinJoins](https://github.com/nopara73/ZeroLink/). WabiSabi enables the construction of trustless coinjoins with arbitrary input and output values.

## Introduction

A Bitcoin transaction consists of inputs and outputs. [CoinJoins](https://bitcointalk.org/index.php?topic=279249.msg2983902#msg2983902) are Bitcoin transactions of multiple collaborating participants.

**CoinJoins are inherently secure**, because if a participant does not see its desired outputs in the resulting CoinJoin transaction, it refuses to sign. A CoinJoin round consists of an **Input Registration**, an **Output Registration** and a **Transaction Signing** phases.

![](https://i.imgur.com/T0FwiZh.png)

**To guarantee network level unlinkability** in regards to input-input, output-output and input-output relations participants must use different anonymity network identities to register each and every inputs and outputs of theirs to a trustless coordinator. This technique leverages an already existing anonymity network infrastructure. Another approach could be what [CoinShuffle](https://petsymposium.org/2014/papers/Ruffing.pdf), [CoinShuffle++](https://www.ndss-symposium.org/wp-content/uploads/2017/09/ndss201701-4RuffingPaper.pdf) and [ValueShuffle](https://www.ndss-symposium.org/wp-content/uploads/2017/09/NDSS-2017_Paper_Ruffing.pdf) took, where a custom anonymity network is created to facilitate the mixing. The anonymity set of this anonymity network is ideally the number of participatns in a round. However in order to avoid cross-mixing round correlation, the use of an existing anonymity network infrastructure is still desirable.

Our proposed scheme generalizes trustless construction of arbitrary CoinJoins. This enables numerous things those were not possible before.

- It enables for the first time trustless construction of [SharedCoin](https://en.bitcoin.it/wiki/Shared_coin) style transactions.
- It enables for the first time a novel mixing construct, called [Knapsack mixing ](https://www.comsys.rwth-aachen.de/fileadmin/papers/2017/2017-maurer-trustcom-coinjoin.pdf).
- It improves upon [Chaumian CoinJoins](https://github.com/nopara73/ZeroLink/) for the first time in numerous ways, most notably it enables the sending of exact amounts and enabling unlinkable consolidation of multiple UTXOs.
- It enables the creation of [CashFusion](https://github.com/cashshuffle/spec/blob/master/CASHFUSION.md) style transactions with orders of magnitude reduction of complexity.
- It can directly improve upon [JoinMarket](https://github.com/JoinMarket-Org/joinmarket-clientserver) by the taker being also the WabiSabi coordinator, so it does not need to learn the mapping of the whole transaction anymore. However this could also be achieved by traditional Chaumian CoinJoins.
- Ideas in this paper can be used to enable construction of CoinJoins with [Confidential Transactions](https://people.xiph.org/~greg/confidential_values.txt). However, unlike WabiSabi, [ValueShuffle](https://www.ndss-symposium.org/wp-content/uploads/2017/09/NDSS-2017_Paper_Ruffing.pdf) is directly built for this purpose.

While network level unlinkability guarantee is easy to achieve, it is difficult to achieve it in a way that also protects against Denial of Service (DoS) attacks. For this reason Chaumian CoinJoins were proposed. However they only work, because the blind signatures provided by the coordinator also correspond with pre-defined denominations. This greatly limits numerous aspects of how a resulting CoinJoin transaction can look like, which has consequences on user experience and on privacy guarantees.

**WabiSabi provides a general and straight forward solution for the problem of trustlessly constructing and coordinating CoinJoins with arbitrary inputs and arbitrary outputs.**

## Overview

The protocol consists of epochs, rounds and phases.

![](https://i.imgur.com/LctiWLS.png)

**Input Registration Phase**, **Output Registration Phase** and **Transaction Signing Phase** follow each other and together they are called the **Main Round**.

**Input Registration** cannot fail, because inputs can be refused or kicked out of the round without penalty before moving on to **Output Registration**. Regardless if **Output Registration** succeeds or fails, the round must progress to **Transaction Signing**, because penalties can only be imposed on malicious inputs and not on malicious outputs. If **Transaction Signing** fails, the protocol progresses to **Blame Round**, which repeats the **Main Round** without the malicious inputs.

The **Main Round** and the potential **Blame Rounds** are called an **Epoch**.

### Main Round

#### Phase 1: Input Registration

An input can be registered if it is confirmed, mature and unspent. An input must be registered along with a proof of ownership, a set of homorphic cryptographic commitments and rangeproofs.

The coordinator replies with a signature to every commitment.

#### Phase 2: Output Registration

The participant unblinds the signatures on the commitments and these signatures must be valid for the underlying values. Thus a participant could register every output separately with different anonymity network identities. The participant must provide a script, a value and the unblinded signature that is valid for the value.

The participant can also combine its signatures in a way that the new signature will be valid for the sum of individual value components.

#### Phase 3: Transaction Signing

Participants sign the final transaction.

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
- Small and fast vs big and slow rounds.
- blind commitment schemes and rangeproofs
