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

Peers register their inputs to the coordinator. Inputs must be registered separately through different anonymtiy network identities, unless an input's amount does not reach the minimum required amount, in which case the input can be registered together with other inputs.

A peer provides a set of inputs along with corresponding proof of ownerships and blind commitments and range proofs. These inputs must be unspent, confirmed and mature. The blinded tokens are blinded random data.

The blind commitments can be created in a flexible way, but standard amounts are recommended. These standard amounts are 1, 10, 100, ... satoshis. Examples:

- If peer's input is 10BTC, then he may choose to register one 10BTC blind commitment or ten 1BTC blind commitments, or even nine 1BTC blind commitments and ten 0.1BTC blind commitments.
- If peer's input is 1.023, then the peer may choose to register one 1BTC blind commitment, two 0.01BTC blind commitments and three 0.001BTC blind commitments.
- If peer's input is 1BTC and the peer would like to send someone else 0.9BTC, then he may register one 0.9BTC and one 0.1BTC blind commitments, or ten 0.1BTC blind commitments.

Furthermore the number of blind commitments a peer sends should be constant, where some of the commitments should be zero commitments as fillers.

Blind commitments are cryptographic commitments those can be signed by the coordinator and later the signature can be unblinded in a way such it is valid for the original amount. Such cryptographic construct should be possible, but further investigation is needed. A concrete example would be using Pedersen commitments to prove the sum of the commitments equal to the input's amount and Bulletproofs to prove they're positive and doesn't create integer overlow. Furthermore these Pedersen commitments shall be constructed in a way that the user can extract a valid unblinded signature to the underlying number of the commitment. More: [Blind Signatures from Knowledge Assumptions](http://www.cs.pwr.edu.pl/hanzlik/preludium/wyniki/paper2.pdf)

#### Phase 2: Payment Registration

Every payment registration request must happen through different anonymity network identities.

Peers register their amounts along with valid signatures and a script. One payment request can contain multiple amounts and signatures, but only one script. This enables merging UTXOs together.

#### Phase 3: Signing

The final transaction is given out to the peers for signing.  

### Blame Round

If an input from an input group did not sign the transaction, then the coordinator prohibits these inputs from participating in followup epochs and the current epoch progresses to a **Blame Round** where only the current epoch's honest peers are permitted to participate.

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
