# WabiSabi - draft v0.2
Adam Ficsor, Yuval Kogman, István András Seres
May 3, 2020

###### Abstract
Chaumian CoinJoin [Miz13][Max13] is a technique used by WasabiWallet and Samourai Wallet [FT17] to facilitate untrusted constructionof collaborative Bitcoin transactions, also known as CoinJoins, by utilizing Chaumian blind signatures [Cha83]. However this technique requiresstandard denominations, which limits how such transactions can be con-structed.
We propose to switch to a Keyed-Verification Anonymous Credentials-based (KVAC) scheme [CPZ19] to enable more flexible transaction styles,like SharedCoin and CashFusion [FL19] style transactions and Knap-sack [MNF17] mixing.  Our generalization also enables consolidation ofUTXOs,  minimizing unmixed change,  relaxing minimum required de-nominations, payments in CoinJoins, better block space efficiency, andPayJoins in CoinJoins. We call this new protocol: WabiSabi

## 1 Protocol Overview
*Note that the following is an incomplete overview, in its current form it only attempts to give some intuition for the cryptographic details described below.*

### 1.1  Roles
In this protocol we define four different roles, three for the user, Alice, Bob, Satoshi, and the coordinator.
- Alice is the role used for registering an input.
- Bob is the role used for registering an output.
- Satoshi is the role used for querying auxiliary data, such as the unsigned transaction.
- The coordinator facilitates the protocol. The protocol’s main goal is to maintain privacy against the coordinator.

Each role must be used with a unique anonymity network identity,and users with multiple inputs or outputs must instantiate multiple Alices or Bobs, one for each input or output respectively.

...

## 2 Cryptographic Details
Following [CPZ19], the scheme is defined in a group $\mathbb{G}$ of prime order $q$, written in multiplicative notation.
We require the following fixed set of group elements:
$$
\[
G_{w}, G_{w^{\prime}}, G_{x_{0}}, G_{x_{1}},
G_{v}, G_{s}, G_g, G_h,
G_{V}.
\]
$$
