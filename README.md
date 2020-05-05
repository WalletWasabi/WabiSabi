**Authors.** István András Seres, nopara73, nothingmuch  
**Acknowledgement.** We thank the numerous contributors to this scheme. We will update the full list of contributors after the paper passes the work in progress status.  
**Status.** Work in progress.

# WabiSabi: Trustless and Arbitrary CoinJoins

## Abstract
[Chaumian CoinJoins](https://github.com/nopara73/ZeroLink/) is a technique used by Wasabi Wallet and Samourai Wallet to facilitate untrusted construction of collaborative Bitcoin transactions, also known as CoinJoins, by utilizing Chaumian blind signatures. However this technique because it reveals to the coordinator input-input and input-output linkage of the user. Furter it requires standard denominations, which limits how such transactions can be constructed.

We propose to switch to a Keyed-Verification Anonymous Credentials-based (KVAC) scheme to enable more flexible transaction styles, like SharedCoin and CashFusion style transactions and Knapsack mixing. Our generalization also enables consolidation of UTXOs, minimizing unmixed change, relaxing minimum required denominations, payments in CoinJoins, better block space efficiency, and PayJoins in CoinJoins. We call this new protocol: WabiSabi.

## Motivation

WabiSabi should improve upon current CoinJoin techniques in the following ways:
- Enable unlinkable consolidation of multiple inputs by one user.
- Enable unlinkable merging of any number of inputs into any number of outputs.
- Enable the creation of arbitrary values outputs.

WabiSabi will enable novel use cases like:
- CoinJoin to the same user
-- Private and efficient consolidation of multiple small value coins.
-- Moving coins between wallets, for example from hot wallet to hardware wallet.
- Payment transactions in the same CoinJoin
-- Private and efficient transaction batching.
-- The sender does not gain knowledge of the input of the receiver.
 
WabiSabi is good for Bitcoin because:
- Improves the privacy of every day users.
- Rewards blockspace efficient transactions [like transaction batching and coin consolidation] with extra privacy.
- Can be combined with other privacy solutions.

## Work in Progress

The current focus of the research are the cryptographic primitives of the communication protocol of user and server.
The detailed explanation can be found in the [source code](main.tex) and [latest release](https://github.com/zkSNACKs/WabiSabi/releases), as well in a [proof of concept implementation](https://github.com/lontivero/WabiSabi).
Nuances of the transaction construction to ensure privacy on the blockchain level is out of scope as of now.

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
