---
abstract: |
    Chaumian CoinJoin [@mizrahi2013blind][@maxwell2013coinjoin] is a
    technique used by Wasabi Wallet and Samourai Wallet [@zerolink] to
    facilitate untrusted construction of collaborative Bitcoin transactions,
    also known as CoinJoins, by utilizing Chaumian blind
    signatures [@chaum1983blind]. However this technique requires standard
    denominations, which limits how such transactions can be constructed.

    We propose to switch to a Keyed-Verification Anonymous Credentials-based
    (KVAC) scheme [@chase2019signal] to enable more flexible transaction
    styles, like SharedCoin and CashFusion [@cashfusion] style transactions
    and Knapsack [@maurer2017anonymous] mixing. Our generalization also
    enables consolidation of UTXOs, minimizing unmixed change, relaxing
    minimum required denominations, payments in CoinJoins, better block
    space efficiency, and PayJoins in CoinJoins. We call this new protocol:
    WabiSabi.
author:
- 'Adam Ficsor, Yuval Kogman, István András Seres'
bibliography:
- 'references.bib'
title: 'WabiSabi - draft v0.2'
---

# Protocol Overview

*Note that the following is an incomplete overview, in its current form
it only attempts to give some intution for the cryptographic details
described below.*

## Roles

In this protocol we define four different roles, three for the user,
Alice, Bob and Satoshi, and the coordinator.

-   Alice is the role used for registering an input.

-   Bob is the role used for registering an output.

-   Satoshi is the role used for querying auxiliary data, such as the
    unsigned transaction.

-   The coordinator facilitates the protocol. The protocol's main goal
    is to maintain privacy against the coordinator.

Each user role needs to be used with a different anonymity network
identity, and users with multiple inputs or outputs must instantiate
multiple Alices or Bobs, one for each input or output respectively.

## Terminology and notation

**Input**: An existing Bitcoin transaction output that the user wants to
use as an input in the CoinJoin transaction. For this section we
represent inputs only in terms of their satoshi amount (integer)
<img src="svgs/eb5b310b0861a7d21d52d358d315f8f0.svg?invert_in_darkmode" align=middle width=20.812609949999988pt height=14.15524440000002pt/>, ignoring details like proving ownership. Details such
as confirmation, standardness and other factors are outside the scope of
this document.

**Output**: A Bitcoin transaction output that the user wishes to create
in the CoinJoin without linking to any of their inputs or other outputs.
Similarly to inputs, we represent outputs only in terms of their satoshi
amount <img src="svgs/d89f46bb2a4366b0ef276461e681c0de.svg?invert_in_darkmode" align=middle width=27.83246399999999pt height=14.15524440000002pt/>, ignoring details like the `scriptPubKey`.

**Credential**: An anonyous credential is issued by the coordinator at
input registration, and certifies attributes that the coordinator
validates before issuing. The user can then prove possession of a valid
credential in zero-knowledge in order to register an output without the
coordinator being able to link it to the input registration from which
it originates, or any other output registrations.

We use the key-verifiable anonymous credential scheme
from [@chase2019signal], instantiated with two group attributes
(attributes whose value is an element of the underlying group
<img src="svgs/a158a43ace9779e0a6109b3c9f9df93d.svg?invert_in_darkmode" align=middle width=12.785434199999989pt height=22.648391699999998pt/>).

**Attribute**: In order to facilitate construction of Bitcoin
transactions, a credential represents some amount of Bitcoin. For this
we use two attributes: <img src="svgs/01a4b8f4a5200d75237824e7fc62c593.svg?invert_in_darkmode" align=middle width=22.93577549999999pt height=22.465723500000017pt/> is a commitment to the amount of the
registered input in satoshis and <img src="svgs/0d5aa2d1b94d707e4359815ea4ba1366.svg?invert_in_darkmode" align=middle width=22.151885249999992pt height=22.465723500000017pt/> is a commitment to a serial
number used for double spending prevention.

During credential presentation randomized versions of the attributes are
presented, which we denote <img src="svgs/e835ae6182f5ee4444b7f209f91df314.svg?invert_in_darkmode" align=middle width=18.73711124999999pt height=22.465723500000017pt/> and <img src="svgs/2b5bd0bee00271e01a0c9266f09fb5c3.svg?invert_in_darkmode" align=middle width=17.95321934999999pt height=22.465723500000017pt/>.

Finally, <img src="svgs/63bb9849783d01d91403bc9a5fea12a2.svg?invert_in_darkmode" align=middle width=9.075367949999992pt height=22.831056599999986pt/> is a protocol level constant, denoting the number of
credentials used in input and output registration requests, and
<img src="svgs/55c0338703d3fde4e92af220b73d7807.svg?invert_in_darkmode" align=middle width=106.77295694999998pt height=26.76175259999998pt/> constrains the amount value ranges[1].

## Input Registration

The user, acting as Alice, submits her input of value <img src="svgs/eb5b310b0861a7d21d52d358d315f8f0.svg?invert_in_darkmode" align=middle width=20.812609949999988pt height=14.15524440000002pt/>
along with <img src="svgs/63bb9849783d01d91403bc9a5fea12a2.svg?invert_in_darkmode" align=middle width=9.075367949999992pt height=22.831056599999986pt/> pairs of group attributes, <img src="svgs/f8eaf029a1c23466bdf17080e14b4775.svg?invert_in_darkmode" align=middle width=76.82477714999999pt height=24.65753399999998pt/>. She
proves in zero knowledge that the sum of the requested sub-amounts is
equal to <img src="svgs/eb5b310b0861a7d21d52d358d315f8f0.svg?invert_in_darkmode" align=middle width=20.812609949999988pt height=14.15524440000002pt/> and that the individual amounts are positive
integers in the allowed range.

The coordinator verifies the proofs, and issues <img src="svgs/63bb9849783d01d91403bc9a5fea12a2.svg?invert_in_darkmode" align=middle width=9.075367949999992pt height=22.831056599999986pt/> MACs (message
authentication codes) on the requested attributes, along with a proof of
knowledge of the secret key as described in *Credential Issuance*
protocol of [@chase2019signal].

## Output Registration

Now acting as Bob, to register her output the user randomizes the
attributes and generates a proof of knowledge of a valid credential
issued by the coordinator.

Additionally, she proves knowledge of representation of the serial
number commitments. These serial numbers are revealed for double
spending protection, but the knowledge of commitment opening should be
done in zero knowledge to avoid revealing the randomness of the original
commitment in the input registration phase or the randomization added in
output registration time.

Finally, she proves that the sum of the randomized amount attributes
<img src="svgs/e835ae6182f5ee4444b7f209f91df314.svg?invert_in_darkmode" align=middle width=18.73711124999999pt height=22.465723500000017pt/> matches the requested output amount <img src="svgs/d89f46bb2a4366b0ef276461e681c0de.svg?invert_in_darkmode" align=middle width=27.83246399999999pt height=14.15524440000002pt/>,
analogously to input registration. Note that there is no need for range
proofs at this phase.

The user submits these proofs, the randomized attributes, and the serial
numbers. The coordinator verifies the proofs, and if it accepts the
output will be included in the transaction.

## Signing phase

The coordinator sends out the final unsigned transaction to the
different Alices who will sign if they see their registered output
included in the transaction.

# Cryptographic Details

Following [@chase2019signal], the scheme is defined in a group
<img src="svgs/a158a43ace9779e0a6109b3c9f9df93d.svg?invert_in_darkmode" align=middle width=12.785434199999989pt height=22.648391699999998pt/> of prime order <img src="svgs/db5fafda9a00f45985de8992d8e78ade.svg?invert_in_darkmode" align=middle width=12.49431149999999pt height=14.15524440000002pt/> written in multiplicative notation.

We require the following fixed set of group elements:
<p align="center"><img src="svgs/ab37becb85e93ffc8077074791d52f44.svg?invert_in_darkmode" align=middle width=276.86548229999994pt height=15.936036599999998pt/></p>

This notation deviates slightly from [@chase2019signal], in that we
subscript the attribute generators <img src="svgs/9a615908a33eb3da7528aa0275517dc4.svg?invert_in_darkmode" align=middle width=23.976908999999992pt height=22.465723500000017pt/> as <img src="svgs/2c307b69121ab0828ffc5d68a1679df8.svg?invert_in_darkmode" align=middle width=19.912904549999993pt height=22.465723500000017pt/> and <img src="svgs/3ab2ecf96ebac62752026ae6627b9e90.svg?invert_in_darkmode" align=middle width=19.129014299999994pt height=22.465723500000017pt/> instead
of using numerical indices, and we require two additional generators
<img src="svgs/bee8c731a74cd7704c2eafecd09eb0d4.svg?invert_in_darkmode" align=middle width=19.75061384999999pt height=22.465723500000017pt/> and <img src="svgs/316af53c14c3acaf3ed626bda068df7e.svg?invert_in_darkmode" align=middle width=20.62067039999999pt height=22.465723500000017pt/> for constructing the attributes <img src="svgs/01a4b8f4a5200d75237824e7fc62c593.svg?invert_in_darkmode" align=middle width=22.93577549999999pt height=22.465723500000017pt/> and <img src="svgs/0d5aa2d1b94d707e4359815ea4ba1366.svg?invert_in_darkmode" align=middle width=22.151885249999992pt height=22.465723500000017pt/> as
Pedersen commitments.

We assume that all generator points used throughout the protocol are
generated in a way that nobody knows the discrete logarithms between any
pair of them.

As with the generators we denote the secret key
<img src="svgs/3fe1ad071b3782621088718f735463db.svg?invert_in_darkmode" align=middle width=184.48633829999997pt height=24.7161288pt/>.

The issuer parameters <img src="svgs/9becce7215859239c9c0509d76a47ebf.svg?invert_in_darkmode" align=middle width=134.2391523pt height=24.65753399999998pt/> are computed as:
<p align="center"><img src="svgs/2ba6dc04e02e0abf7dfcc5b71fa01343.svg?invert_in_darkmode" align=middle width=341.95868685pt height=38.332593749999994pt/></p>

## Input Registration

Alice wants to register an input UTXO with value <img src="svgs/eb5b310b0861a7d21d52d358d315f8f0.svg?invert_in_darkmode" align=middle width=20.812609949999988pt height=14.15524440000002pt/>,
broken into sub-amounts <img src="svgs/9f7365802167fff585175c1750674d42.svg?invert_in_darkmode" align=middle width=12.61896569999999pt height=14.15524440000002pt/> where <img src="svgs/9219d656a69a6a954702d1975d0dc357.svg?invert_in_darkmode" align=middle width=59.48726684999998pt height=24.65753399999998pt/>. She
submits amount and serial number commitments:
<p align="center"><img src="svgs/03045821029020722351edf470b5842b.svg?invert_in_darkmode" align=middle width=204.65491694999997pt height=17.031940199999998pt/></p>
<p align="center"><img src="svgs/3fdaa5377d342a7b72387c5d0dee4915.svg?invert_in_darkmode" align=middle width=203.4847749pt height=17.031940199999998pt/></p>

For each amount she includes a range proof:
<p align="center"><img src="svgs/1c709f5b8e4010981c2b18e3fc8d4c12.svg?invert_in_darkmode" align=middle width=422.81582309999993pt height=17.55700485pt/></p>

Alice also needs to convince the coordinator that the sent amount
commitments add up to the registered input UTXO value, hence she sends
the following proof: <p align="center"><img src="svgs/be830071fdb241e3333ab535a96fe027.svg?invert_in_darkmode" align=middle width=102.41465684999999pt height=47.93392394999999pt/></p>

The coordinator can then calculate the product of the amount commitments
and check:

<p align="center"><img src="svgs/69add9b15b04f3dba660ae7de0e58279.svg?invert_in_darkmode" align=middle width=159.76596074999998pt height=47.93392394999999pt/></p>

Note that this equality over the product of commitments implies the sum
is correct: <p align="center"><img src="svgs/c773fe1363cb89af9d7c18f791a52e9a.svg?invert_in_darkmode" align=middle width=349.68493724999996pt height=47.93392394999999pt/></p>

If the coordinator accepts it issues the credentials by responding with
a MAC
<img src="svgs/e4586ed2c5a8c62771893fd4b31a7e25.svg?invert_in_darkmode" align=middle width=174.6275355pt height=24.65753399999998pt/>
for each credential where
<img src="svgs/f759f8b8e47f26afa605994597d98431.svg?invert_in_darkmode" align=middle width=128.16510795pt height=22.648391699999998pt/> and
<p align="center"><img src="svgs/4bf346ba726750ee274de6392f68fcd2.svg?invert_in_darkmode" align=middle width=203.7604635pt height=18.401823pt/></p>

To avoid tagging individual users the coordinator must also prove
knowledge of the secret key, and that <img src="svgs/2fb34d64b1ed37eb410b3c76810f991a.svg?invert_in_darkmode" align=middle width=70.5644775pt height=24.65753399999998pt/> is correct
relative to <img src="svgs/9d6085c164b4ed27f7c25d2e9caadf05.svg?invert_in_darkmode" align=middle width=134.2391523pt height=24.65753399999998pt/> with the following proof of
knowledge:

<p align="center"><img src="svgs/c3e8a360c92b855cd2facb11ab854f48.svg?invert_in_darkmode" align=middle width=324.36691319999994pt height=120.4487064pt/></p>

## Output Registration

After the input registration the user may have up to <img src="svgs/4f4f4e395762a3af4575de74c019ebb5.svg?invert_in_darkmode" align=middle width=5.936097749999991pt height=20.221802699999984pt/> credentials
from all of her input registration requests made as one or more Alice
identities. Let <img src="svgs/81c24e2d90171569caf37138fceb7974.svg?invert_in_darkmode" align=middle width=63.53865044999999pt height=24.65753399999998pt/> be the indices of
credentials that she wants to consolidate into a single output
registration.

### Credential validity

For each credential <img src="svgs/a9632a01526dc9b26bf61b382c164bbf.svg?invert_in_darkmode" align=middle width=36.781765349999986pt height=22.465723500000017pt/> Bob executes the <img src="svgs/f8336050aaa87118dda2feeca5d8928b.svg?invert_in_darkmode" align=middle width=36.84942689999999pt height=22.831056599999986pt/> protocol
as in [@chase2019signal].

First, she chooses <img src="svgs/93d2ca33eacb2c125b743bf05193756a.svg?invert_in_darkmode" align=middle width=61.389074999999984pt height=22.648391699999998pt/>, and computes
<img src="svgs/ba441d6ad552995864e2b9188de063aa.svg?invert_in_darkmode" align=middle width=133.50283979999998pt height=24.65753399999998pt/> and the randomized commitments:

<p align="center"><img src="svgs/4982aa46c42ea71c97354a03a3ec9099.svg?invert_in_darkmode" align=middle width=119.5400052pt height=120.26603324999998pt/></p>

To prove to the coordinator that she is in posession of a valid MAC on
her amount and serial number commitments, Bob computes the following
proof of knowledge: <p align="center"><img src="svgs/6f028789d108f6f9fc8f330b3595dc09.svg?invert_in_darkmode" align=middle width=288.4765587pt height=72.7423125pt/></p>

Finally, Bob sends
<img src="svgs/484c691d6ce71938f048df067f1418dc.svg?invert_in_darkmode" align=middle width=215.54675009999997pt height=27.53345100000001pt/>
to the coordinator, who computes:
<p align="center"><img src="svgs/f3750b99de17e9141af32a8a33b028ab.svg?invert_in_darkmode" align=middle width=227.76908714999996pt height=40.385761349999996pt/></p>

using the secret key <img src="svgs/c32ea36d2ce75556cb23ce67d37c4e11.svg?invert_in_darkmode" align=middle width=121.5717063pt height=24.65753399999998pt/> and verifies
<img src="svgs/09abc9e7ef0473fb358d066523da0cb4.svg?invert_in_darkmode" align=middle width=42.84862394999999pt height=27.53345100000001pt/>.

### Over-spending prevention by proving sum of amounts

The product of randomized commitments amounts to:

<p align="center"><img src="svgs/47dbcb234027fcffa738934dfaaba4b6.svg?invert_in_darkmode" align=middle width=394.04431769999997pt height=37.775108249999995pt/></p>

Therefore we can obtain a witness-indistinguishable proof for the sum of
the committed values <img src="svgs/9f7365802167fff585175c1750674d42.svg?invert_in_darkmode" align=middle width=12.61896569999999pt height=14.15524440000002pt/> in the randomized commitments:

<p align="center"><img src="svgs/be63249ede360e6be0d159ddc9215e95.svg?invert_in_darkmode" align=middle width=176.90002439999998pt height=49.315569599999996pt/></p>

The coordinator checks whether <p align="center"><img src="svgs/291006c4c20473e8bb8631bbdbc9eef0.svg?invert_in_darkmode" align=middle width=250.45641554999997pt height=37.8240456pt/></p>

The coordinator can compute the right hand side of the verification
equation, since she obtained the exponents of each of the generator
points from the submitted <img src="svgs/5f26abceba37132fcbda96286df7d518.svg?invert_in_darkmode" align=middle width=33.89872529999999pt height=21.839370299999988pt/>. Informally soundness of the
proof system holds as user does not know the discrete logs between the
generator points used in the randomized commitments. While
zero-knowledge is ensured since <img src="svgs/abaa001e2638cf195486306683530fff.svg?invert_in_darkmode" align=middle width=55.41907304999998pt height=24.657735299999988pt/> does not leak
anything about individual <img src="svgs/6af8e9329c416994c3690752bde99a7d.svg?invert_in_darkmode" align=middle width=12.29555249999999pt height=14.15524440000002pt/>. We can have a similar argument for
<img src="svgs/aadeb43b5222eb26051a45e8d75e6af4.svg?invert_in_darkmode" align=middle width=61.500760199999995pt height=24.657735299999988pt/> and <img src="svgs/e426fa6cbb1b435396a6ef5db6705e02.svg?invert_in_darkmode" align=middle width=18.377239649999993pt height=14.15524440000002pt/>.

### Double-spending prevention by revealing serial numbers

Bob randomizes her serial number commitments:

<p align="center"><img src="svgs/4d26c3d2b93bb67a078d00111ae550bc.svg?invert_in_darkmode" align=middle width=291.2009991pt height=16.826464050000002pt/></p>

Bob proves knowledge of representation of her submitted randomized
serial number commitments, namely:
<p align="center"><img src="svgs/688ca1c5af3957c9bb9522757793fd59.svg?invert_in_darkmode" align=middle width=344.44499595pt height=19.4813124pt/></p>

where the serial number <img src="svgs/4fa3ac8fe93c68be3fe7ab53bdeb2efa.svg?invert_in_darkmode" align=middle width=12.35637809999999pt height=14.15524440000002pt/> is a public input, revealed to prevent
double spending. The coordinator checks that the <img src="svgs/4fa3ac8fe93c68be3fe7ab53bdeb2efa.svg?invert_in_darkmode" align=middle width=12.35637809999999pt height=14.15524440000002pt/> have not been
used before (but allowing for idempotent output registration).

Note that after revealing <img src="svgs/4fa3ac8fe93c68be3fe7ab53bdeb2efa.svg?invert_in_darkmode" align=middle width=12.35637809999999pt height=14.15524440000002pt/>, we no longer have perfect hiding in the
<img src="svgs/da6b8a96fc9646c1856fb76cb2456665.svg?invert_in_darkmode" align=middle width=26.537410349999988pt height=22.465723500000017pt/> commitment, since, because there is exactly one
<img src="svgs/6515cffe407d004a59e357f5bb6735d1.svg?invert_in_darkmode" align=middle width=57.13793909999999pt height=22.648391699999998pt/> such that
<img src="svgs/901791ebf84e966872e49c5a410c6483.svg?invert_in_darkmode" align=middle width=121.16664779999999pt height=24.246581700000014pt/>. To preserve user privacy in
case of a crypto break we can add another randomness term with an
additional generator to the the serial number commitment.

[1] <img src="svgs/3a9f2d6b1cade1d64ae926c51d915e1e.svg?invert_in_darkmode" align=middle width=224.04163814999995pt height=24.65753399999998pt/>
