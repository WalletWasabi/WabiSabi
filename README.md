**Authors.** Isvan Seres Andras, nopara73, nothingmuch

# Yohohoho

Generalization of [Chaumian CoinJoins](https://github.com/nopara73/ZeroLink/). Enabling the construction of trustless coinjoins with arbitrary output amounts using a novel combination of three cryptographic primitives: a homomorphic cryptographic commitment scheme, range proofs and a blind signature scheme that is based on the chosen commitment scheme.

## Background

A Bitcoin transaction consists of inputs and outputs. CoinJoins[REFERENCE] are Bitcoin transactions where multiple participants collaborate on. This construction is inherently secure, because if a participant does not see its desired outputs in the resulting CoinJoin, it refuses to sign its provided inputs.

However it does not inherently guarantee unlinkability. To make the sub-transactions of CoinJoins unlinkable during the transaction's construction some approaches have been proposed:

 **SharedCoin**[REFERENCE] was Blockchain.info's CoinJoin implementation, where the participants of the CoinJoins did not learn the sub-transactions, but Blockchain.info, the leader of the CoinJoin did.

**JoinMarket**[REFERENCE] is a concept and implementation where the participants of the CoinJoin is divided to two distinct groups: a taker and one or more makers. In this scheme the makers do not, but the taker do learn the sub-transactions. Unlike SharedCoin, the taker, the leader of the CoinJoin is not constant, so there is no single entity that knows all sub-transactions of all JoinMarket style CoinJoins. Furtheremore the taker is also incentivized to not reveal the mappings.[REFERENCE]

**Chaumian CoinJoin**[REFERENCE] is a mechanism where neither the participants, nor the coordinator learns the sub-transactions. It achieves it by the participants communicating with the coordinator through multiple anonymity network identities within a single round.

**CoinShuffle**[REFERENCE] and **CoinShuffle++**[REFERENCE] took the approach of building their own custom-made anonymity network, thus guaranteeing anonymity within the set of participants.

While only Chaumian CoinJoin requires leveraging an anonymity network, the rest of the schemes can benefit from it, too. In SharedCoin if the participants would constantly use anonymity network identities, the leader would not be able to link the participants to IP addresses, thus real world identities. The same, even more applies to JoinMarket and CoinShuffle constructions, as they not only have to hide real world identities from the bulletin boards they utilize, but also from other participants. In case of JoinMarket it's only the taker. However in order to avoid cross-transaction linkability in the sense that nobody should learn which pseudonyms participate in which transactions, it is also important to change anonymity network identities between every transaction.  

Furthermore notice that both JoinMarket and SharedCoin could use the constructions of Chaumian CoinJoin or CoinShuffle to facilitate unlinkability. However in case of SharedCoin, if it would use any of these constructions, then it would be equivalent to these constructions, while in case of JoinMarket, since the linkability is mitigated in other means, the complexity may not be justified.

In this paper we build upon Chaumian CoinJoins. While Chaumian CoinJoins guarantee unlinkability through changing anonymity network identities, it also utilizes a blind signature scheme to prevent denial of service attacks. However in its current form it only works, because the blind signatures provided by the coordinator also correspond with pre-defined denominations. This works for self mixing, because anonymity is provided by making outputs equal, however it prohibits creating arbitrary output amounts, which is essential for making payments in coinjoins. In such cases anonymity can be achieved in different ways, like making the inputs equal, mixing on the change output or through utilizing Knapsack Mixing[REFERENCE]. Furthermore such scheme would also enable unlinkability on all inputs of a participant and gain computational privacy against subset sum attacks by the coordinator[REFERENCE]. Another issue that comes up with Chaumian CoinJoins is the consolidations of many UTXOs it creates. Solving the provlem also enables consolidating them in a private way, at least on the transaction construction level.

**Thus the question is: how to construct CoinJoins where participants can can create arbitrary outputs without anyone learning the neither complete nor partial mapping of the sub-transactions?**

## Overview

The protocol consists of epochs, rounds and phases.

![](https://i.imgur.com/dAr56jm.png)

**Input Registration Phase**, **Payment Registration Phase** and **Signing Phase** follow each other and together they are called the **Main Round**.

**Input Registration Phase** cannot fail, because inputs can be refused or kicked out of the round without penalty before moving on to **Payment Registration Phase**. Regardless if **Payment Registration Phase** succeeds or fails, the round must progress to **Signing Phase**, because penalties can only be imposed on malicious inputs and not on malicious outputs. If **Signing Phase** fails, the protocol progresses to **Blame Round**, which repeats the **Main Round** without the malicious inputs.

The **Main Round** and the potential **Blame Rounds** are called an **Epoch**.

### Main Round

#### Phase 1: Input Registration

Peers register their inputs to the coordinator. Inputs must be registered separately through different anonymtiy network identities, unless an input's amount does not reach the minimum required amount, in which case the input can be registered together with other inputs.

A peer provides a set of inputs along with corresponding proof of ownerships and blind commitments and range proofs. These inputs must be unspent, confirmed and mature. The blinded tokens are blinded random data.

The blind commitments can be created in a flexible way, but standard amounts are recommended. These standard amounts are 1, 10, 100, ... satoshis. Examples:

- If peer's input is 10BTC, then he may choose to register 1 10BTC blind commitment or 10 1BTC blind commitments, or even 9 1BTC blind commitments and 10 0.1BTC blind commitments.
- If peer's input is 1.023, then the peer may choose to register 1 1BTC blind commitment, 2 0.01BTC blind commitments and 3 0.001BTC blind commitments.
- If peer's input is 1BTC and the peer would like to send someone else 0.9BTC, then he may register a 0.9BTC and a 0.1BTC blind commitments, or 10 0.1BTC blind commitments.

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
