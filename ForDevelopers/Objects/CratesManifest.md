# CRATES – MANIFEST
[Go to Contents](https://github.com/Fr8org/Fr8Core.NET/blob/master/README.md)  
A Manifest is a simple json schema that defines the contents of a Crate of data. Use of Manifests is optional. They’re essentially a way for activities to effectively process data. Put another way, if you know the Manifest of a Crate, you can use it to deserialize the contents (which is just a big json string) into structured data.

While any builder of Fr8 Actions can defined private Manifests for their own purposes, the real value of Manifests comes through a shared registry. For example, in the [Fr8 Manifest Registry](https://github.com/Fr8org/Fr8Core.NET/blob/master/ForDevelopers/.md), Id 14 is assigned to a Manifest called DocuSignEnvelope that looks like this:

![manifest_docusign_envelopes](https://github.com/Fr8org/Fr8Core.NET/blob/master/img/CratesManifest_ManifestDocusignEnvelopes.png) 

The Fr8 Company maintains a public registry of Manifests at fr8.co/registry. Anyone can register a new Manifest, although registration of manifests that are equivalent or highly similar to existing manifests is discouraged. The registry generates a unique id that can be used in Crates.

An Action designer can design an Action that works with DocuSignEnvelopes. They can assume that inbound crates that contain DocuSignEnvelope data will deserialize into this object.

Manifests are versioned, and will get generally get richer and more comprehensive over time.

 

### Special Manifests

The Baseline Manifests: Standard Payload Data and Standard Configuration Fields

If there isn’t a more targeted manifest that fits, these are the fallback. Both essentially are a simple List of JSON objects, which can have arbitrarily complex values.

[Go to Contents](https://github.com/Fr8org/Fr8Core.NET/blob/master/README.md) 
